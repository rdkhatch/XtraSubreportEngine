using System;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Contracts.RuntimeActions
{
    public interface IReportRuntimeAction<in T> : IReportRuntimeAction
        where T : XRControl
    {
        Func<T, bool> Predicate { get; }
        new Action<T> ActionToApply { get; }
    }

    public interface IReportRuntimeAction
    {
        Func<XRControl, bool> ActionPredicate { get; }
        Action<XRControl> ActionToApply { get; }
        Type ApplyToControlType { get; }
    }
}
