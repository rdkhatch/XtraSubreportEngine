using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using NUnit.Framework;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Runtime.UnitTests
{
    [TestFixture]
    public class ApiTweaks
    {
        [Test]
        public void Should_pass_root_hashcode()
        {
            var view = new XtraReport {DataSource = new[] {new object(), new object()}};

            var detailBand = new DetailBand();
            var container = new XRSubreport();
            var subReport = new MyReportBase();

            container.ReportSource = subReport;
            detailBand.Controls.Add(container);
            view.Bands.Add(detailBand);

            IReportController myController = new XRReportController(view);
            Action<XtraReport> printAction = r => r.ExportToMemory();
            var newView = myController.Print(printAction);

            var subReportsHashcode =
                ((MyReportBase) ((XRSubreport) newView.Bands[BandKind.Detail].Controls[0]).ReportSource).RootHashCode;

            newView.RootHashCode.Should().NotBe(0);

            subReportsHashcode.Should().Be(newView.RootHashCode);

        }

        [Test]
        public void Should_not_collide_with_two_controllers()
        {
            var view = new XtraReport {DataSource = new[] {new object(), new object()}};

            var counterA = 0;
            var counterB = 0;

            var actionA = ReportRuntimeAction<XRControl>.WithNoPredicate(c => counterA++);
            var actionB = ReportRuntimeAction<XRControl>.WithNoPredicate(c => counterB++);
            var facadeA = new XRRuntimeActionFacade(actionA);
            var facadeB = new XRRuntimeActionFacade(actionB);

            var controllerA = new XRReportController(view, facadeA);
            var controllerB = new XRReportController(view, facadeB);

            controllerA.Print(r => r.ExportToMemory());
            controllerB.Print(r => r.ExportToMemory());

            counterA.Should().Be(1);
            counterB.Should().Be(1);
        }

        [Test]
        public void Should_Dispose_visitors() // no memory leaks here!
        {
            var view = new XtraReport {DataSource = new[] {new object(), new object()}};
            var controllerA = new XRReportController(view);
            var view2 = controllerA.Print(r => r.ExportToMemory());
            
            GlobalXRSubscriber.Singleton.Visitors.Values.Count(wr => wr.IsAlive && ((XRRuntimeVisitor)wr.Target).ReportHashcode == view2.RootHashCode).Should().Be(1);
            GC.Collect();
            GlobalXRSubscriber.Singleton.Visitors.Values.Count(wr => wr.IsAlive && ((XRRuntimeVisitor)wr.Target).ReportHashcode == view2.RootHashCode).Should().Be(0);
        }
        

    }

}