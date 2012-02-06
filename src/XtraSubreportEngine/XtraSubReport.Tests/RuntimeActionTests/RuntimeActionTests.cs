using System.Drawing;
using System.Linq;
using DevExpress.XtraReports.UI;
using NUnit.Framework;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReport.Tests
{
    // TODO: Use .repx for tests.  Right now, we are using direct references.  We should use .repx, which is what we will REALLY be using.

    [TestFixture]
    public class RuntimeActionTests
    {
        [Test]
        public void change_all_label_text()
        {
            var transformText = "Jeremiah";
            var action = new ReportRuntimeActionBase<XRLabel>((l) => true, (l) => l.Text = transformText);

            var label = new XRLabel() { Text = string.Empty };

            var report = new ReportFactory().GetNewReport();
            report.Bands[0].Controls.Add(label);

            var subscriber = XRRuntimeSubscriber.SubscribeWithActions(action);

            TestHelper.RunReport(report);

            Assert.AreEqual(transformText, label.Text);
        }

        [Test]
        public void predicate_prevents_applying_action()
        {
            var transformText = "Jeremiah";
            var action = new ReportRuntimeActionBase<XRLabel>((l) => l.Text != string.Empty, (l) => l.Text = transformText);

            var label1 = new XRLabel() { Text = string.Empty };
            var label2 = new XRLabel() { Text = "ChangeMe" };

            var report = new ReportFactory().GetNewReport();
            report.Bands[0].Controls.Add(label1);
            report.Bands[0].Controls.Add(label2);

            var subscriber = XRRuntimeSubscriber.SubscribeWithActions(action);

            TestHelper.RunReport(report);

            Assert.AreNotEqual(transformText, label1.Text);
            Assert.AreEqual(transformText, label2.Text);
        }

        [Test]
        public void applies_to_entire_report()
        {
            var color = Color.Green;
            var action = new ReportRuntimeActionBase<XtraReport>((r) => true, (r) => r.BackColor = color);

            var report = new ReportFactory().GetNewReport();

            var subscriber = XRRuntimeSubscriber.SubscribeWithActions(action);

            TestHelper.RunReport(report);

            Assert.AreEqual(color, report.BackColor);
        }

        [Test]
        public void applies_to_subreports()
        {
            var tuple = TestHelper.GetParentAndNestedSubreport();

            var parentReport = tuple.Item1;
            var subreportContainer = tuple.Item2;
            var subreport = tuple.Item3;

            var myLabel = new XRLabel();
            var subreportBand = subreport.Bands.Cast<Band>().First();
            subreportBand.Controls.Add(myLabel);

            var color = Color.Green;
            var action = new ReportRuntimeActionBase<XRLabel>((label) => true, (label) => label.BackColor = color);

            var subscriber = XRRuntimeSubscriber.SubscribeWithActions(action);

            TestHelper.RunReport(parentReport);

            Assert.AreEqual(color, myLabel.BackColor);

            // TODO: Is this test good enough?
            // This didn't work when we tried it in our DevExpress presentation.
            //Assert.Fail();
        }

        [Test]
        public void applies_to_tables()
        {
            var transformColor = Color.Blue;
            var action = new ReportRuntimeActionBase<XRControl>(c => true, c => c.BackColor = transformColor);

            var table = new XRTable();
            var row = new XRTableRow();
            var cell = new XRTableCell();
            row.Cells.Add(cell);
            table.Rows.Add(row);

            var report = new ReportFactory().GetNewReport();
            report.Bands[0].Controls.Add(table);

            var subscriber = XRRuntimeSubscriber.SubscribeWithActions(action);

            TestHelper.RunReport(report);

            Assert.AreEqual(transformColor, cell.BackColor);
        }

    }
}
