using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubreport.Engine.RuntimeActions
{
    public interface IReportRuntimeActionProvider
    {
        IEnumerable<IReportRuntimeAction> GetRuntimeActions();
    }
}
