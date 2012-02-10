using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using GeniusCode.Framework.Extensions;
using TechTalk.SpecFlow;
using XtraSubReports.Runtime.UnitTests;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Runtime.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Subreport is passed correct datasource")]
    public class SubReportIsPassedCorrectDatasource
    {
        private XtraReport _parentReport;
        private string _subReportFilePath;
        private XRSubreport _xrSubreportContainer;
        private MyReportBase _newParentReport;
        private Func<XtraReport, XRSubreport> _getContainerFunc;
        private MyReportBase _newSubReport;
        private XRSubreport _newSubReportContainer;
        private IRuntimeActionFacade _actionFacade;
        private GlobalXRSubscriber _subscriber;
        private int _counter;

        private readonly HashSet<Person> _datasources = new HashSet<Person>();
        private DataSourceTrackingController _controller;

        [Given(@"A parent report exists")]
        public void GivenAParentReportExists()
        {
            _parentReport = new XtraReport();
        }

        [Given(@"the parent report has a datasource of three items")]
        public void GivenTheParentReportHasADatasource()
        {
            _parentReport.DataSource = new List<Person>
                                           {
                                               new Person {Name = "Douglas Sam", Age = 17},
                                               new Person {Name = "Fred Thomas", Age = 35},
                                               new Person {Name = "Alex Matthew", Age = 100}
                                           };
        }

        [Given(@"a subreport exists as a file")]
        public void GivenASubreportExistsAsAFile()
        {
            _subReportFilePath = Helpers.GetNewTempFile() + ".repx";
            Path.GetDirectoryName(_subReportFilePath).Should().NotBeNullOrEmpty();
            var subReport = new XtraReport();
            subReport.SaveLayout(_subReportFilePath);
            File.Exists(_subReportFilePath).Should().BeTrue();
        }


        [Given(@"a XRSubreport container exists in the parent report's (.*) band")]
        public void GivenTheXRSubreportContainerExistsInTheParentReportBand(string location)
        {
            const string nameToUse = "SubReportContainer";

            Band band;
            switch (location.ToUpper())
            {
                case "HEADER":  
                    band = new ReportHeaderBand();
                    _getContainerFunc = r => (XRSubreport)r.Bands[BandKind.ReportHeader].Controls[nameToUse];
                    break;
                case "FOOTER":
                    band = new ReportFooterBand();
                    _getContainerFunc = r => (XRSubreport) r.Bands[BandKind.ReportFooter].Controls[nameToUse];
                    break;
                case "DETAIL":
                    band = new DetailBand();
                    _getContainerFunc = r => (XRSubreport) r.Bands[BandKind.Detail].Controls[nameToUse];
                    break;
                default:
                    throw new NotImplementedException();
            }

            _parentReport.Bands.Add(band);
            _xrSubreportContainer = new XRSubreport {Name = nameToUse};
            band.Controls.Add(_xrSubreportContainer);

        }

        [Given(@"the XRSubreport container references the subreport's filename")]
        public void GivenTheXRSubreportContainerReferencesTheSubreportSFilename()
        {
            _xrSubreportContainer.ReportSourceUrl = _subReportFilePath;
        }

        [Given(@"the xtrasubreport engine is initialized")]
        public void GivenTheXtrasubreportEngineIsInitialized()
        {
            _controller = new DataSourceTrackingController(_parentReport,(s,o)=> _counter++);
        }


        [Given(@"the xtrasubreport engine is initialized with datasource tracking")]
        public void GivenTheXtrasubreportEngineIsInitializedWithDataSourceTracking()
        {

            

            _controller = new DataSourceTrackingController(_parentReport,(s,ds) =>
                                                             {
                                                                 _counter++;

                                                                 Person person = null;

                                                                 ds.TryAs<Person>(p => person = p);
                                                                 ds.TryAs<List<object>>(
                                                                     list => person = (Person) list.SingleOrDefault());

                                                                 if(person != null)
                                                                     _datasources.Add(person);
                                                                 else
                                                                 {
                                                                     throw new NotImplementedException();
                                                                 }

                                                             });
        }

        


        [When(@"the report engine runs")]
        public void WhenTheReportEngineRuns()
        {
            _newParentReport = _controller.Print(r => r.ExportToMemory());

            _newSubReportContainer = _getContainerFunc(_newParentReport);
        }


        [Then(@"the subreport's datasource should be the same as the parent report's datasource")]
        public void ThenTheSubreportSDatasourceShouldBeTheSameAsTheParentReportSDatasource()
        {
            _newSubReport = (MyReportBase)_newSubReportContainer.ReportSource;

            var parentList = (List<Person>)_newParentReport.DataSource;
            var childList = (List<Person>)_newSubReport.DataSource;

            parentList.Should().Equal(childList);
        }

        [Then(@"the subreport's datasource should not be null")]
        public void TheSubReportsDatasourceShouldNotBeNull()
        {
            _newSubReport.DataSource.Should().NotBeNull();
        }

        

        [Then(@"the subreport action should have been fired (.*) time\(s\)")]
        public void ThenTheSubreportActionShouldHaveBeenFired(int times)
        {
            _counter.Should().Be(times);
        }

        [Then(@"each item in the parent's datasource should have been set once")]
        public void ThenEachItemInTheParentSDatasourceShouldHaveBeenSetOnce()
        {
            _datasources.Count.Should().Be(3);
        }




    }

    public class Person
    {
        public string Name;
        public int Age;
    }
}
