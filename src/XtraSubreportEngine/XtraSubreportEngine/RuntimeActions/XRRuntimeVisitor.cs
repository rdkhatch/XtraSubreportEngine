using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using GeniusCode.Framework.Extensions;
using XtraSubreport.Engine.Eventing;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class XRRuntimeVisitor : IDisposable 

{
    private readonly IEventAggregator _eventAggregator;
        private readonly MyReportBase _report;

        #region Constructor

        public int ReportHashcode
        {
            get { return _report.RootHashCode; }
        }

    public XRRuntimeVisitor(IEventAggregator eventAggregator, MyReportBase report)
    {
        _eventAggregator = eventAggregator;
        _report = report;
    }

        #endregion

    #region Attach Handlers

    public void Visit()
    {
        Visit(_report);
    }

    private void AttachToControl(XRControl control)
    {
        control.BeforePrint -= control_BeforePrint;
        control.BeforePrint += control_BeforePrint;
    }

    private void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        Visit((XRControl) sender);
    }

    #endregion

    #region Visitors

    // Main Loop
    private void Visit(XRControl control)
    {
        // Set Root Hashcode On SubReport Here:
        control.TryAs<XRSubreport>(sr => RunTimeHelper.SetRootHashCodeOnSubreport(sr));


        // Self
        PublishScopedMessage(control);

        // Get Children
        var children = VisitChildren(control);

        // Recursion
        foreach (var child in children)
            //Visit(child);
            AttachToControl(child);
    }

    private void PublishScopedMessage(XRControl control)
    {
        var hashcode = ((MyReportBase) control.Report).RootHashCode;
        var message = new ScopedXRControlBeforePrintMessage(hashcode, control);
        _eventAggregator.Publish(message);
    }

    private List<XRControl> VisitChildren(XRControl control)
    {
        if (control is XRSubreport) return VisitSubreportPlaceholderChildren(control as XRSubreport);
        if (control is XRTable) return VisitTableChildren(control as XRTable);
        if (control is XRTableRow) return VisitTableRowChildren(control as XRTableRow);
        if (control is Band) return VisitBandChildren(control as Band);

        return control.Controls.Cast<XRControl>().ToList();
    }

    private List<XRControl> VisitBandChildren(Band band)
    {
        

        // Special Controls - Bands & Subreport Placeholders (which need additional event handlers)
        var childBands = band.Controls.OfType<Band>().ToList();
        var subreportPlaceholders = band.Controls.OfType<XRSubreport>().ToList();

        // Attach to Special Controls
        childBands.ForEach(AttachToControl);  //BUG: Not firing
        subreportPlaceholders.ForEach(AttachToControl);

        var ignore = childBands.Concat(subreportPlaceholders.Cast<XRControl>());

        // Normal Controls
        return band.Controls.Cast<XRControl>().Except(ignore).ToList();
    }

    private List<XRControl> VisitSubreportPlaceholderChildren(XRSubreport placeholder)
    {
        // Subreport
        XtraReport subreport = placeholder.ReportSource;
        Visit(subreport);

        // Return empty collection
        return new List<XRControl>();
    }

    private List<XRControl> VisitTableChildren(XRTable table)
    {
        // XRTableRows
        return table.Rows.Cast<XRControl>().ToList();
    }

    private List<XRControl> VisitTableRowChildren(XRTableRow row)
    {
        // XRTableCells
        return row.Cells.Cast<XRControl>().ToList();
    }

    #endregion

        public void Dispose()
        {
            //TODO: Remove event handlers
        }
}

}
