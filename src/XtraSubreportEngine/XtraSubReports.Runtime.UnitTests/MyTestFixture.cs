using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using NUnit.Framework;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.Eventing;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Runtime.UnitTests
{
    [TestFixture]
    public class MyTestFixture
    {
        private int _counter;
        private XRRuntimeActionController _actionController;
        private XRRuntimeSubscriber _subscriber;

        [Test]
        public void Handler_wireup_should_be_predicatable()
        {
            _actionController = new XRRuntimeActionController(new PassSubreportDataSourceRuntimeAction((s,ds) => _counter++));
            _subscriber = new XRRuntimeSubscriber(_actionController);

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


            var hi = myBase.CloneUsingLayout();
            hi.ExportToMemory();


            var message = new XRBeforePrintMessage(new MyReportBase(), null);
            EventAggregator.Singleton.Publish(message);

            _counter.Should().Be(4);
        }


        public class Hi
        {
            
        }

    }
}
