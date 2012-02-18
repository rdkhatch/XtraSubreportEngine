using System.Collections.Generic;
using System.Linq;

namespace XtraSubreport.Contracts.RuntimeActions
{
    public interface IReportRuntimeActionProvider
    {
        IEnumerable<IReportRuntimeAction> GetRuntimeActions();
    }
}
