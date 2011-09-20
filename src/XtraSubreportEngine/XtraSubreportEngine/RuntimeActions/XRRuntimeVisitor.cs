using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class XRRuntimeVisitor
    {
        private IRuntimeActionController _controller;

        #region Constructor

        public XRRuntimeVisitor(IRuntimeActionController controller)
        {
            _controller = controller;
        }

        #endregion

        #region Attach Handlers

        //public virtual void AttachTo(XtraReport report)
        //{
        //    // Important:
        //    // Event handlers DO NOT WORK in the end user designer - because of DevExpress serialization / CodeDom
        //    // http://www1.devexpress.com/Support/Center/p/Q256674.aspx
        //    // http://www.devexpress.com/Support/Center/p/Q240047.aspx
        //    // Must either:
        //    // 1.) Override Before_Print within custom reports class (ie, MyReportBase)
        //    // 2.) Use Scripts create an event handler

        //    AttachTo(report as Band);
        //}

        private void AttachTo(Band band)
        {
            band.BeforePrint += (s, e) =>
                {
                    Visit(band);
                };
        }

        private void AttachTo(XRSubreport placeholder)
        {
            placeholder.BeforePrint += (s, e) =>
            {
                Visit(placeholder);
            };
        }

        #endregion

        // Main Loop
        #region Visitors

        public void Visit(XRControl control)
        {
            // Self
            _controller.AttemptActionsOnControl(control);

            // Get Children
            var children = VisitChildren(control);

            // Recursion
            foreach (var child in children)
                Visit(child);
        }

        private IEnumerable<XRControl> VisitChildren(XRControl control)
        {
            if (control is Band) return VisitBandChildren(control as Band);
            if (control is XRSubreport) return VisitSubreportChildren(control as XRSubreport);
            if (control is XRTable) return VisitTableChildren(control as XRTable);
            if (control is XRTableRow) return VisitTableRowChildren(control as XRTableRow);

            return control.Controls.Cast<XRControl>();
        }

        private IEnumerable<XRControl> VisitBandChildren(Band band)
        {
            // Special Controls - Bands & Subreport Placeholders (which need additional event handlers)
            var childBands = band.Controls.OfType<Band>().ToList();
            var subreportPlaceholders = band.Controls.OfType<XRSubreport>().ToList();

            // Attach to Controls
            childBands.ForEach(childBand => AttachTo(childBand));
            subreportPlaceholders.ForEach(placeholder => AttachTo(placeholder));

            var ignore = Enumerable.Concat(childBands.Cast<XRControl>(), subreportPlaceholders.Cast<XRControl>());

            // Normal Controls
            return band.Controls.Cast<XRControl>().Except(ignore);
        }

        private IEnumerable<XRControl> VisitSubreportChildren(XRSubreport placeholder)
        {
            // Subreport
            var subreport = placeholder.ReportSource;
            AttachTo(subreport);

            // Return empty collection
            yield break;
        }

        private IEnumerable<XRControl> VisitTableChildren(XRTable table)
        {
            return table.Rows.Cast<XRTableRow>();
        }

        private IEnumerable<XRControl> VisitTableRowChildren(XRTableRow row)
        {
            return row.Cells.Cast<XRTableCell>();
        }

        #endregion

    }

}
