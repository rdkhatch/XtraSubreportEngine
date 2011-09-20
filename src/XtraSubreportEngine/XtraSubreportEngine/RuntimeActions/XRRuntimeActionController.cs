using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class XRRuntimeActionController : IRuntimeActionController
    {
        private List<IReportRuntimeAction> _runtimeActions;

        public XRRuntimeActionController(IReportRuntimeActionProvider actionProvider)
            : this(actionProvider.GetRuntimeActions().ToArray())
        {
        }

        public XRRuntimeActionController(IEnumerable<IReportRuntimeActionProvider> actionProviders)
            : this(actionProviders.SelectMany(provider => provider.GetRuntimeActions()).ToArray())
        {
        }

        public XRRuntimeActionController(params IReportRuntimeAction[] actions)
        {
            _runtimeActions = new List<IReportRuntimeAction>(actions);
        }

        public void AttemptActionsOnControl(XRControl control)
        {
            // TODO: Add Filter by Whitelist ReportActionName and/or ReportActionGroupName

            // Predicates
            var actions = from action in _runtimeActions
                          where action.ActionPredicate(control)
                          select action;

            // Execute matching Runtime Actions
            foreach (var action in actions)
                action.ActionToApply.Invoke(control);
        }
    }
}