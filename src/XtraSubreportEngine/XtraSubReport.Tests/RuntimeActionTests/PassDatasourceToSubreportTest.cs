using System.Collections;
using System.Linq;
using DevExpress.XtraReports.UI;
using NUnit.Framework;
using XtraSubreport.Engine;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReport.Tests.RuntimeActions
{
    [TestFixture]
    public class PassDatasourceToSubreportTest
    {
        [Test]
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
            var passSubreportAction = new PassSubreportDataSourceRuntimeAction();

            var subscriber = XRRuntimeSubscriber.SubscribeWithActions(passSubreportAction);

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
