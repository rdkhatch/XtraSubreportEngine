using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class XRRuntimeVisitor
    {
        private readonly IRuntimeActionController _controller;

        #region Constructor

        public XRRuntimeVisitor(IRuntimeActionController controller)
        {
            _controller = controller;
        }

        #endregion

        #region Attach Handlers

        private void AttachTo(XtraReport report)
        {
            Visit(report);
        }

        private void AttachToControl(XRControl control)
        {
            control.BeforePrint -= control_BeforePrint;
            control.BeforePrint += control_BeforePrint;

/*            //TODO: Check that this doesn't fire too many times
            control.BeforePrint += (s, e) => Visit(control);*/
        }

        void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Visit((XRControl)sender);
        }

        #endregion

        #region Visitors

        // Main Loop
        public void Visit(XRControl control)
        {
            // Self
            _controller.AttemptActionsOnControl(control);

            // Get Children
            var children = VisitChildren(control);

            // Recursion
            // TODO: There is a bug where controls get called WAY more than they should. Why?
            foreach (var child in children)
                //Visit(child);
                AttachToControl(child);
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
            childBands.ForEach(AttachToControl);
            subreportPlaceholders.ForEach(AttachToControl);

            var ignore = childBands.Concat(subreportPlaceholders.Cast<XRControl>());

            // Normal Controls
            return band.Controls.Cast<XRControl>().Except(ignore).ToList();
        }

        private List<XRControl> VisitSubreportPlaceholderChildren(XRSubreport placeholder)
        {
            // Subreport
            XtraReport subreport = placeholder.ReportSource;
            AttachTo(subreport);

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

    }

}
