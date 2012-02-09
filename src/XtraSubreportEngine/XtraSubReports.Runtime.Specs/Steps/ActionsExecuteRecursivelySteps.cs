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
using XtraSubreport.Engine;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Runtime.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Actions")]
    public class ActionsExecuteRecursivelySteps
    {
        private XtraReport _report;
        private XRLabel _changeMeLabel;
        private XRLabel _dontChangeMeLabel;
        private IReportRuntimeAction _action;
        private int _counter;
        private MyReportBase _newReport;
        private XRLabel _newChangeMeLabel;
        private XRLabel _newDontChangeMeLabel;
        private XRReportController _controller;

        [Given(@"A report exists")]
        public void GivenAReportExists()
        {
            _report = new XtraReport();
            _report.Bands.Add(new ReportHeaderBand());

        }

        [Given(@"the report has an XRLabel named ChangeMe in the header")]
        public void GivenTheReportHasAnXRLabelNamedChangeMeInTheHeader()
        {
            _changeMeLabel = new XRLabel {Name = "ChangeMe"};
            _report.Bands[BandKind.ReportHeader].Controls.Add(_changeMeLabel);
        }

        [Given(@"ChangeMe's text property has a value of (.*)")]
        public void GivenChangeMeSTextPropertyHasAValueOfBrodie(string value)
        {
            _changeMeLabel.Text = value;
        }

        [Given(@"the report has another XRLabel named DontChangeMe in header")]
        public void GivenTheReportHasAnotherXRLabelNamedDontChangeMeInHeader()
        {
            _dontChangeMeLabel = new XRLabel {Name = "DontChangeMe"};
            _report.Bands[BandKind.ReportHeader].Controls.Add(_dontChangeMeLabel);
        }

        [Given(@"DontChangeMe's text property has a value of (.*)")]
        public void GivenDontChangeMeSTextPropertyHasAValueOfGreenBayPackers(string value)
        {
            _dontChangeMeLabel.Text = value;
        }

        [Given(@"an action exists against an XRLabel named ChangeMe to change the name to (.*) and increment a counter")]
        public void GivenAnActionExistsAgainstAnXRLabelNamedChangeMe(string newName)
        {
            _action = new ReportRuntimeAction<XRLabel>(l => l.Name == "ChangeMe", l =>
                                                                                      {
                                                                                          l.Text = newName;
                                                                                          _counter++;
                                                                                      });
        }

        [Given(@"the xtrasubreport engine is initialized")]
        public void GivenTheXtrasubreportEngineIsInitialized()
        {
            var facade = new XRRuntimeActionFacade(_action);
            _controller = new XRReportController(_report, facade);
            
        }

        [When(@"the report engine runs")]
        public void WhenTheReportEngineRuns()
        {
            _newReport = _controller.Print(r => r.ExportToMemory());

            _newChangeMeLabel = (XRLabel)_newReport.Bands[0].Controls[_changeMeLabel.Name];
            _newDontChangeMeLabel = (XRLabel) _newReport.Bands[0].Controls[_dontChangeMeLabel.Name];
        }


        [Then(@"ChangeMe's text property should have a value of (.*)")]
        public void ThenChangeMeSTextPropertyShouldHaveAValueOfCamp(string value)
        {
            _newChangeMeLabel.Text.Should().Be(value);

        }

        [Then(@"DontChangeMe's text property should have a value of (.*)")]
        public void ThenDontChangeMeSTextPropertyShouldHaveAValueOfGreenBayPackers(string value)
        {
            _newDontChangeMeLabel.Text.Should().Be(value);
        }


        [Then(@"the counter incremented by the action should have a count of 1")]
        public void ThenTheCounterIncrementedByTheActionShouldHaveACountOf1()
        {
            _counter.Should().Be(1);
        }
    }
}
