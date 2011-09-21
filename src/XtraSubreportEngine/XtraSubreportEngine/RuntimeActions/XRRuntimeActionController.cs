using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class XRRuntimeActionController : IRuntimeActionController
    {
        private List<IReportRuntimeAction> _runtimeActions;
        private List<Type> _controlTypes;

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

            _controlTypes = (from action in _runtimeActions
                             select action.ApplyToControlType).Distinct().ToList();
        }

        public void AttemptActionsOnControl(XRControl control)
        {
            // TODO: Add Filter by Whitelist ReportActionName and/or ReportActionGroupName

            // Optimization - ignore XRControls that we don't have ReportActions for
            var foundMatchingRuntimeAction = (from type in _controlTypes
                                              where type.IsAssignableFrom(control.GetType())
                                              select type).Any();

            if (foundMatchingRuntimeAction == false)
                return;

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