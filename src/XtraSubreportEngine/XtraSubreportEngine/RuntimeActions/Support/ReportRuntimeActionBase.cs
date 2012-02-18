using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubreport.Engine.RuntimeActions.Support
{
    public abstract class ReportRuntimeActionBase<T> : ReportRuntimeActionBase, IReportRuntimeAction<T>
        where T : XRControl
    {

        protected sealed override bool ReturnShouldApplyAction(XRControl control)
        {
            return this.ReturnShouldApplyAction((T)control);
        }

        protected virtual bool ReturnShouldApplyAction(T control)
        {
            return true;
        }

        Func<T, bool> IReportRuntimeAction<T>.Predicate
        {
            get { return this.ReturnShouldApplyAction; }
        }

        protected sealed override void PerformAction(XRControl control)
        {
            this.PerformAction((T)control);
        }

        protected abstract void PerformAction(T control);

        Action<T> IReportRuntimeAction<T>.ActionToApply
        {
            get { return this.PerformAction; }
        }

        protected sealed override Type GetActionTargetType()
        {
            return typeof(T);
        }
    }

    public abstract class ReportRuntimeActionBase : IReportRuntimeAction
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        protected virtual bool ReturnShouldApplyAction(XRControl control)
        {
            return true;
        }

        Func<XRControl, bool> IReportRuntimeAction.ActionPredicate
        {
            get { return ReturnShouldApplyAction; }
        }

        protected abstract void PerformAction(XRControl control);

        public Action<XRControl> ActionToApply
        {
            get { return PerformAction; }
        }

        protected abstract Type GetActionTargetType();

        public Type ApplyToControlType
        {
            get { return GetActionTargetType(); }
        }
    }
}