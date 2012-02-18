using System.Linq;
using System;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubreport.Engine.RuntimeActions.Support
{
    public sealed class ReportRuntimeAction<T> : ReportRuntimeActionBase<T> where T : XRControl
    {
        private readonly Func<T, bool> _predicate;
        private readonly Action<T> _action;

        public static ReportRuntimeAction<T> WithNoPredicate(Action<T> action)
        {
            return new ReportRuntimeAction<T>(a => true, action);
        }

        public ReportRuntimeAction(Func<T, bool> predicate, Action<T> action)
        {
            _predicate = predicate;
            _action = action;
        }

        protected override bool ReturnShouldApplyAction(T control)
        {
            return _predicate(control);
        }

        protected override void PerformAction(T control)
        {
            _action(control);
        }
    }
}
