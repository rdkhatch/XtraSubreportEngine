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

        [Test]
        public void Should_convert_subreport_to_myReportBase()
        {
            var report = new XtraReport();
            var detailBand = new DetailBand();
            var subReportContainer = new XRSubreport {ReportSource = new XtraReport()};
            report.Bands.Add(detailBand);
            detailBand.Controls.Add(subReportContainer);


            var controller = new XRReportController(report);
            var newReport = controller.Print(p=> p.ExportToMemory());

            var newContainer = (XRSubreport) newReport.Bands[0].Controls[0];
            newContainer.ReportSource.GetType().Should().Be(typeof (MyReportBase));

        }

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

        [Test]
        public void Should_handle_detail_reports_with_subreport()
        {
            var textvalues = new List<Tuple<int,string>>();
            var report = new XtraReportWithSubReportInDetailReport();
            report.DataSource = new List<Person2>
                                    {
                                        new Person2
                                            {
                                                Name = "Douglas Sam",
                                                Age = 17,
                                                Dogs = new List<Dog> {new Dog {Name = "Rex"}, new Dog {Name = "Rudy"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Fred Thomas",
                                                Age = 35,
                                                Dogs =
                                                    new List<Dog> {new Dog {Name = "Sally"}, new Dog {Name = "Stubert"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Alex Matthew",
                                                Age = 100,
                                                Dogs =
                                                    new List<Dog>
                                                        {new Dog {Name = "Nibbles"}, new Dog {Name = "Norbert"}}
                                            }

                                    };
            int counter = 0;
            var action = new ReportRuntimeAction<XRLabel>(l =>
                                                              {
                                                                  counter++;
                                                                  textvalues.Add(new Tuple<int, string>(l.Report.GetHashCode(), l.Text));
                                                              });
            var facade = new XRRuntimeActionFacade(action);

            var c = new XRReportController(report, facade);
            var newReport = c.Print(a=> a.ExportToMemory());
            //Not safe for batch test runs GlobalXRSubscriber.Singleton.Visitors.Count.Should().Be(2);
            counter.Should().Be(6);


        }

        [Test]
        public void Should_handle_detail_reports()
        {
            var textvalues = new List<Tuple<int, string>>();
            var report = new XtraReportWithLabelInDetailReport();
            report.DataSource = new List<Person2>
                                    {
                                        new Person2
                                            {
                                                Name = "Douglas Sam",
                                                Age = 17,
                                                Dogs = new List<Dog> {new Dog {Name = "Rex"}, new Dog {Name = "Rudy"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Fred Thomas",
                                                Age = 35,
                                                Dogs =
                                                    new List<Dog> {new Dog {Name = "Sally"}, new Dog {Name = "Stubert"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Alex Matthew",
                                                Age = 100,
                                                Dogs =
                                                    new List<Dog>
                                                        {new Dog {Name = "Nibbles"}, new Dog {Name = "Norbert"}}
                                            }

                                    };
            int counter = 0;
            var action = new ReportRuntimeAction<XRLabel>(l =>
            {
                counter++;
                textvalues.Add(new Tuple<int, string>(l.Report.GetHashCode(), l.Text));
            });
            var facade = new XRRuntimeActionFacade(action);

            var c = new XRReportController(report, facade);
            var newReport = c.Print(a => a.ExportToMemory());

            counter.Should().Be(6);


        }

        [Test]
        public void XtraReport_should_fire_event_properly()
        {
            var report = new XtraReportWithSubReportInDetailReport();
            report.DataSource = new List<Person2>
                                    {
                                        new Person2
                                            {
                                                Name = "Douglas Sam",
                                                Age = 17,
                                                Dogs = new List<Dog> {new Dog {Name = "Rex"}, new Dog {Name = "Rudy"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Fred Thomas",
                                                Age = 35,
                                                Dogs =
                                                    new List<Dog> {new Dog {Name = "Sally"}, new Dog {Name = "Stubert"}}
                                            },
                                        new Person2
                                            {
                                                Name = "Alex Matthew",
                                                Age = 100,
                                                Dogs =
                                                    new List<Dog>
                                                        {new Dog {Name = "Nibbles"}, new Dog {Name = "Norbert"}}
                                            }

                                    };
            var detailReportBand = report.Controls.OfType<DetailReportBand>().Single();
            var sr = (XRSubreport) detailReportBand.Bands[BandKind.Detail].Controls[0];
            var label = (XRLabel)sr.ReportSource.Bands[BandKind.Detail].Controls[0];
            var counter = 0;
            label.BeforePrint += (s, o) => counter++;
            report.ExportToMemory();
            counter.Should().Be(6);
        }

    }
}
