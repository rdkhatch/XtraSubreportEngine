using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Contracts.RuntimeActions
{
    public interface IReportRuntimeAction<T> : IReportRuntimeAction
        where T : XRControl
    {
        Func<T, bool> Predicate { get; }

        Action<T> ActionToApply { get; }
    }

    public interface IReportRuntimeAction
    {
        Func<XRControl, bool> ActionPredicate { get; }

        Action<XRControl> ActionToApply { get; }
    }
}
