using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Contracts.RuntimeActions
{
    public class ReportRuntimeActionBase<T> : IReportRuntimeAction<T>
        where T:XRControl
    {
        protected Func<T, bool> _predicate;
        protected Action<T> _action;

        protected ReportRuntimeActionBase()
        {
            // If you want to inherit, you can
        }

        public ReportRuntimeActionBase(Func<T, bool> predicate, Action<T> action) : this()
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
                        return _predicate.Invoke((T) control);

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
    }
}
