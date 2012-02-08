using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using NUnit.Framework;
using XtraSubreport.Engine.Eventing;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Runtime.UnitTests
{
    [TestFixture]
    public class MyTestFixture
    {
        private int _counter;
        private XRRuntimeActionFacade _actionFacade;
        private GlobalXRSubscriber _subscriber;

        [Test]
        public void Handler_wireup_should_be_predicatable()
        {
            var myBase = new MyReportBase();
            var detailBand = new DetailBand();
            var container = new XRSubreport();
            var subReport = new MyReportBase();

            container.ReportSource = subReport;
            detailBand.Controls.Add(container);
            myBase.Bands.Add(detailBand);

            myBase.DataSource = new[]
                                    {
                                        new object(),
                                        new object(),
                                        new object(),
                                        new object()
                                    };


            var controller = new DataSourceTrackingController(myBase, (s, ds) => _counter++);
            controller.Print(r => r.ExportToMemory());
            _counter.Should().Be(4);
        }

    }
}
