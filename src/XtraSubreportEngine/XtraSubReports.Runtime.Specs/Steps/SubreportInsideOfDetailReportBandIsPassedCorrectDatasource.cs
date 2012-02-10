using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using TechTalk.SpecFlow;
using XtraSubReports.Runtime.UnitTests;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Runtime.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Subreport Inside of a Detail Report Band is passed the correct Datasource")]
    public class SubreportInsideOfDetailReportBandIsPassedCorrectDatasource
    {
        private XtraReport _parentReport;
        private string _subReportFilePath;
        private XRSubreport _xrSubreportContainer;
        private int _counter;

        
        private readonly List<object> _datasources = new List<object>();
        private DataSourceTrackingController _controller;
        
        private DetailReportBand _detailReport;
        private MyReportBase _newParentReport;

        [Given(@"A parent report exists")]
        public void GivenAParentReportExists()
        {
            _parentReport = new XtraReport();
        }

        [Given(@"the parent report has a datasource of three items")]
        public void GivenTheParentReportHasADatasource()
        {
            _parentReport.DataSource = new List<Person2>
                                           {
                                               new Person2 {Name = "Douglas Sam", Age = 17,
                                               Dogs = new List<Dog> { new Dog { Name = "Rex"}, new Dog {Name = "Rudy"} }},
                                               new Person2 {Name = "Fred Thomas", Age = 35,
                                               Dogs = new List<Dog> { new Dog { Name = "Sally"}, new Dog {Name = "Stubert"} }},
                                               new Person2 {Name = "Alex Matthew", Age = 100,
                                               Dogs = new List<Dog> { new Dog { Name = "Nibbles"}, new Dog {Name = "Norbert"} }}
                                           };
        }

        [Given(@"a subreport exists as a file")]
        public void GivenASubreportExistsAsAFile()
        {
            _subReportFilePath = Helpers.GetNewTempFile() + ".repx";
            Path.GetDirectoryName(_subReportFilePath).Should().NotBeNullOrEmpty();
            var subReport = new MyReportBase();
            subReport.SaveLayout(_subReportFilePath);
            File.Exists(_subReportFilePath).Should().BeTrue();
        }


        [Given(@"the parent report has a detail report band with a datamember of dogs")]
        public void GivenTheParentReportHasADetailReportBandWithADatamemberOfDogs()
        {
            _parentReport.Bands.Add(new DetailBand());
            _detailReport = new DetailReportBand();
            _detailReport.DataMember = "Dogs";
            _detailReport.Level = 0;
            _detailReport.Name = "DetailReport";

            _parentReport.Bands.Add(_detailReport);
        }

        [Given(@"the detail report band contains a subreport in its (.*) band")]
        public void GivenTheDetailReportBandContainsASubreportInItsHeaderBand(string location)
        {
            Band band;
            switch (location.ToUpper())
            {
                case "HEADER":  
                    band = new ReportHeaderBand();
                    break;
                case "FOOTER":
                    band = new ReportFooterBand();
                    break;
                case "DETAIL":
                    band = new DetailBand();
                    break;
                default:
                    throw new NotImplementedException();
            }

            
            _xrSubreportContainer = new XRSubreport();
            band.Controls.Add(_xrSubreportContainer);
            _detailReport.Bands.Add(band);
        }

        [Given(@"the XRSubreport container references the subreport's filename")]
        public void GivenTheXRSubreportContainerReferencesTheSubreportSFilename()
        {
            _xrSubreportContainer.ReportSourceUrl = _subReportFilePath;
        }

        [Given(@"the xtrasubreport engine is initialized")]
        public void GivenTheXtrasubreportEngineIsInitialized()
        {
            _controller = new DataSourceTrackingController(_parentReport,(s,o)=>
                                                                             {
                                                                                 _counter++;
                                                                                 _datasources.Add(o);
                                                                             });
        }

        [When(@"the report engine runs")]
        public void WhenTheReportEngineRuns()
        {
            _newParentReport = _controller.Print(r => r.ExportToMemory());
        }

        [Then(@"the subreport should have the same datasource as the containing group's datasource collection")]
        public void ThenTheSubreportShouldHaveTheSameDatasourceAsTheContainingGroupSDatasourceCollection()
        {
            var q = _datasources.OfType<List<Dog>>().SelectMany(t => t);
            q.Count().Should().Be(6);
            _counter.Should().Be(3);
        }

        [Then(@"each subreport should have a datasource containing a single item")]
        public void ThenEachSubreportShouldHaveACollectionDatasourceContainingOnlyOneItem()
        {
            _datasources.OfType<Dog>().Count().Should().Be(6);
        }

        [Then(@"each subreport datasource contains the same datasource as the containing group's detail band")]
        public void ThenEachSubreportDatasourceContainsTheSameDatasourceAsTheContainingGroupSDetailBand()
        {
            var datasources = _datasources.OfType<Dog>().ToList();

            var mainds = (List<Person2>) _parentReport.DataSource;

            mainds[0].Dogs[0].Should().BeSameAs(datasources[0]);
            mainds[0].Dogs[1].Should().BeSameAs(datasources[1]);
            mainds[1].Dogs[0].Should().BeSameAs(datasources[2]);
            mainds[1].Dogs[1].Should().BeSameAs(datasources[3]);
            mainds[2].Dogs[0].Should().BeSameAs(datasources[4]);
            mainds[2].Dogs[1].Should().BeSameAs(datasources[5]);
        }


    }

    public class Person2
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public List<Dog> Dogs { get; set; }
    }

    public class Dog
    {
        public string Name { get; set; }
    }

}
