using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using XtraSubreport.Engine.Extensions;
using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubreport.Engine.RuntimeActions
{

    public class XRRuntimeVisitor
    {
        public List<IReportRuntimeAction> RuntimeActions { get; private set; }
        public Func<XtraReport, bool> AggregatorReportPredicate { get; set; }

        #region Constructors
        public XRRuntimeVisitor(IEnumerable<IReportRuntimeActionProvider> actionProviders, IEnumerable<IReportRuntimeAction> actions)
        {
            RuntimeActions = new List<IReportRuntimeAction>();

            // Actions
            if (actions != null)
                RuntimeActions.AddRange(actions);

            // Action Providers
            if (actionProviders != null)
                actionProviders.ToList().ForEach(provider =>
                    {
                        var actionList = provider.GetRuntimeActions();
                        RuntimeActions.AddRange(actionList);
                    });

        }

        public XRRuntimeVisitor(IEnumerable<IReportRuntimeActionProvider> actionProviders)
            : this(actionProviders, null)
        {
        }

        public XRRuntimeVisitor(IEnumerable<IReportRuntimeAction> actions)
            : this(null, actions)
        {
        }

        public XRRuntimeVisitor(IReportRuntimeAction action)
            : this(new IReportRuntimeAction[] { action })
        {
        }

        #endregion

        /// <summary>
        /// Allows this Visitor to listen to any MyReportBase's BeforePrint event.
        /// Important because DevExpress uses CodeDom and serialization for report scripting etc.  Attaching to BeforePrint events will not work.  You must attach to the Aggregator instead.
        /// </summary>
        /// <param name="aggregatorReportPredicate"></param>
        public virtual void AttachToAggregator(Func<XtraReport, bool> aggregatorReportPredicate)
        {
            AggregatorReportPredicate = aggregatorReportPredicate;

            XRRuntimeVisitorAggregator.BeforePrint += (s, e) =>
                {
                    var report = e.Report;
                    bool attach = AggregatorReportPredicate(report);
                    if (attach)
                        Visit(report);
                };
        }

        public virtual void AttachTo(XtraReport report)
        {
            // Important:
            // Event handlers DO NOT WORK in the end user designer - because of DevExpress serialization / CodeDom
            // http://www1.devexpress.com/Support/Center/p/Q256674.aspx
            // http://www.devexpress.com/Support/Center/p/Q240047.aspx
            // Must either:
            // 1.) Override Before_Print within custom reports class (ie, MyReportBase)
            // 2.) Use Scripts create an event handler

            AttachTo(report as Band);
        }

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

        // Main Loop
        private void Visit(XRControl control)
        {
            // Self
            AttemptActionsOnControl(control);

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

        private void AttemptActionsOnControl(XRControl control)
        {
            // Predicates
            var actions = from action in RuntimeActions
                          where action.ActionPredicate(control)
                          select action;

            // Execute matching Runtime Actions
            foreach (var action in actions)
                action.ActionToApply.Invoke(control);
        }

    }

}
