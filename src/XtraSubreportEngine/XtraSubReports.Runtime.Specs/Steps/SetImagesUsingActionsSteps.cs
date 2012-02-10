using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using TechTalk.SpecFlow;
using XtraSubReports.Runtime.UnitTests;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReports.Runtime.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Images Should be Set By Action")]
    public class SetImagesUsingActionsSteps
    {
        private XtraReport _report;
        private string _imageFileName;
        private XRPictureBox _imageContainer;
        private ReportRuntimeAction<XRPictureBox> _action
            ;

        private string _filename2;

        [Given(@"a report exists")]
        public void GivenAReportExists()
        {
            _report = new XtraReport();
            _report.DataSource = new[] {new object(), new object()};
        }

        [Given(@"an image exists as a file")]
        public void GivenAnImageExistsAsAFile()
        {
            var stream = GetType().Assembly.GetManifestResourceStream("XtraSubReports.Runtime.Specs.Steps.Penguins.jpg");

            _imageFileName = Helpers.GetNewTempFile() + ".jpg";

            using (Stream file = File.OpenWrite(_imageFileName))
            {
                Helpers.CopyStream(stream, file);
            }
        }

        [Given(@"the report contains an image placeholder")]
        public void GivenTheReportContainsAnImagePlaceholder()
        {
            var detail = new DetailBand();
            _imageContainer = new XRPictureBox {Name = "Penguins"};
            detail.Controls.Add(_imageContainer);
            _report.Bands.Add(detail);
        }


        [Given(@"an action exists to place the image into the placeholder")]
        public void GivenAnActionExistsToPlaceTheImageIntoThePlaceholder()
        {
            _action = new ReportRuntimeAction<XRPictureBox>(p => p.ImageUrl = _imageFileName);
        }

        [When(@"the report runs")]
        public void WhenTheReportRuns()
        {
            _filename2 = Helpers.GetNewTempFile() + ".html";
            var controller = new XRReportController(_report, new XRRuntimeActionFacade(_action));
            controller.Print(p => p.ExportToHtml(_filename2));
        }

        [Then(@"the image should be placed into the report")]
        public void ThenTheImageShouldBePlacedIntoTheReport()
        {
            var text = File.ReadAllText(_filename2);
            var toFind = string.Format("<img alt=\"\" src=\"{0}\"", _imageFileName);
            text.Contains(toFind).Should().BeTrue();
        }


    }
}
