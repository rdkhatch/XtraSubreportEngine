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

/*    [TestFixture]
    public class RuntimeActionTests
    {
/*        [Test]
        public void change_all_label_text()
        {
            var transformText = "Jeremiah";
            var action = new ReportRuntimeAction<XRLabel>((l) => true, (l) => l.Text = transformText);

            var label = new XRLabel() { Text = string.Empty };

            var report = new ReportFactory().GetNewReport();
            report.Bands[0].Controls.Add(label);

            //var subscriber = XRRuntimeSubscriber.SubscribeWithActions(action);

            TestHelper.RunReport(report,action);

            Assert.AreEqual(transformText, label.Text);
        }#1#


/*        [Test]
        public void applies_to_subreports()
        {
            var tuple = TestHelper.GetParentAndNestedSubreport();

            var parentReport = tuple.Item1;
            //var subreportContainer = tuple.Item2;
            var subreport = tuple.Item3;

            var myLabel = new XRLabel();
            var subreportBand = subreport.Bands.Cast<Band>().First();
            subreportBand.Controls.Add(myLabel);

            XRLabel newLabel = null;

            var color = Color.Green;
            var action = new ReportRuntimeAction<XRLabel>((label) => true, label =>
                                                                               {
                                                                                   label.BackColor = color;
                                                                                   newLabel = label;
                                                                               });

            TestHelper.RunReport(parentReport,action);

            Assert.AreEqual(color, newLabel.BackColor);

            // TODO: Is this test good enough?
            // This didn't work when we tried it in our DevExpress presentation.
            //Assert.Fail();
        }#1#



    }*/
}
