using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReports.Runtime.UnitTests
{
    public class DataSourceTrackingController : XRReportController
    {
        private readonly Action<XRSubreport, Object> _increment;

        public DataSourceTrackingController(XtraReport view, Action<XRSubreport,Object> increment)
            : base(view)
        {
            _increment = increment;
        }

        protected override IEnumerable<IReportRuntimeAction> OnGetDefautActions()
        {
            yield return new PassSubreportDataSourceRuntimeAction((s, o) => _increment(s,o));
        }

        public void TestPrint()
        {
            Print(r => r.ExportToMemory());
        }
    }
}