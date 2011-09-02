using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevExpress.XtraReports.UI;
using XtraSubreport.Engine.RuntimeActions;
using System.Collections;
using XtraSubreport.Engine.Extensions;

namespace XtraSubReport.Tests.RuntimeActions
{
    [TestClass]
    public class PassDatasourceToSubreportTest
    {
        [TestMethod]
        public void Runtime_datasource_passes_to_subreport()
        {
            var tuple = TestHelper.GetParentAndNestedSubreport();

            var parentReport = tuple.Item1;
            var subreportContainer = tuple.Item2;

            // Run report -> Assert datasource passed
            Run_Report_Assert_Subreport_Received_Datasource(parentReport, subreportContainer);
        }

        private void Run_Report_Assert_Subreport_Received_Datasource(XtraReport parentReport, XRSubreport placeholder)
        {
            var visitor = new XRRuntimeVisitor(new PassSubreportDataSourceRuntimeAction());
            visitor.AttachTo(parentReport);

            var band = placeholder.Band;

            placeholder.AfterPrint += (s, e) =>
            {
                var subreport = placeholder.ReportSource;
                var bandDatasource = band.GetDataSource();
                var subreportDatasource = subreport.DataSource;

                Assert.IsNotNull(bandDatasource);
                Assert.IsNotNull(subreportDatasource);

                if (band is DetailBand)
                    // If DetailBand, datasource collection should only contain a single item
                    subreportDatasource = (subreport.DataSource as IEnumerable).Cast<object>().Single();

                Assert.AreSame(bandDatasource, subreportDatasource);
            };

            // Run report
            TestHelper.RunReport(parentReport);
        }
    }
}
