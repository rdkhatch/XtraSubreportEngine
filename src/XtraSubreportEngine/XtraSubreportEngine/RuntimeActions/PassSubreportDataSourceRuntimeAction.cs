using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Native;
using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class PassSubreportDataSourceRuntimeAction : ReportRuntimeAction<XRSubreport>
    {
        private static Func<XRSubreport, bool> predicate = (subreport) => true;
        private static Action<XRSubreport> action = (subreport) =>
            {
                RunTimeHelper.SetDataSourceOnSubreport(subreport);
            };

        public PassSubreportDataSourceRuntimeAction()
            : base( predicate, action)
        {
        }

    }
}
