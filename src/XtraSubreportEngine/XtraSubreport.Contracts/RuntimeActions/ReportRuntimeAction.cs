using System;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Contracts.RuntimeActions
{

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

    /*    public class ReportRuntimeAction2<T> : IReportRuntimeAction<T>
            where T : XRControl
        {
            private Func<T, bool> _predicate;
            private readonly Action<T> _action;

            protected ReportRuntimeAction2()
            {
                // If you want to inherit, you can
            }

            public ReportRuntimeAction2(Action<T> action)
                : this(a=> true, action)
            {
            }

            public ReportRuntimeAction2(Func<T, bool> predicate, Action<T> action)
                : this()
            {
                _predicate = predicate;
                _action = action;
            }

            Func<T, bool> IReportRuntimeAction<T>.Predicate
            {
                get { return _predicate; }
            }

            Action<T> IReportRuntimeAction<T>.ActionToApply
            {
                get { return _action; }
            }

            public Func<XRControl, bool> ActionPredicate
            {
                get
                {
                    return (control) =>
                    {
                        if (control is T)
                            return _predicate.Invoke((T)control);

                        return false;
                    };
                }
                set
                {
                    _predicate = value;
                }
            }

            public Action<XRControl> ActionToApply
            {
                get
                {
                    return (control) =>
                    {
                        if (control is T)
                            _action.Invoke((T)control);
                        else
                        {
                            string errorMessage = String.Format("Cannot execute Report RuntimeAction. Control {0} is not of type {1}", control.ToString(), typeof(T).ToString());
                            throw new ArgumentException(errorMessage);
                        }
                    };
                }
            }


            public Type ApplyToControlType
            {
                get { return typeof(T); }
            }
        }*/
}
