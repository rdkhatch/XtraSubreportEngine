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
    [Scope(Feature = "Subreport Recieves Datasource With Collection Path Traversal")]
    public class SubReportRecievesDatasourceWithPathTraversal
    {
        private XtraReport _parentReport;
        private string _subReportFilePath;
        private XRSubreport _xrSubreportContainer;
        private int _counter;

        private readonly HashSet<Person2> _personDataSources = new HashSet<Person2>();
        private readonly HashSet<Dog> _dogDatasources = new HashSet<Dog>();
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


        [Given(@"the parent report has a group traversal on a collection inside a detail band")]
        public void GivenTheParentReportHasAGroupTraversalOnACollectionInsideADetailBand()
        {

            _detailReport = new DetailReportBand();
            _detailReport.DataMember = "Dogs";
            _detailReport.DataSource = _parentReport.DataSource;
            _detailReport.Level = 0;
            _detailReport.Name = "DetailReport";

            _parentReport.Bands.Add(_detailReport);
        }

        [Given(@"the group contains a subreport in its (.*) band")]
        public void GivenTheXRSubreportContainerExistsInTheParentReportBand(string location)
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

                                                                                 o.TryAs<Dog>(
                                                                                     d => _dogDatasources.Add(d));

                                                                                 o.TryAs<Person2>(p=> _personDataSources.Add(p));


                                                                                 o.TryAs<List<object>>(ol =>
                                                                                                           {
                                                                                                               ol.OfType<Person2>().ForAll(p => _personDataSources.Add(p));
                                                                                                               ol.OfType<Dog>().ForAll(d => _dogDatasources.Add(d));
                                                                                                           });

                                                                                 o.TryAs<List<Person2>>(ol => ol.ForEach(p=> _personDataSources.Add(p)));
                                                                                 o.TryAs<List<Dog>>(ol => ol.ForEach(d => _dogDatasources.Add(d)));
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
            _dogDatasources.Count.Should().Be(6);
            _personDataSources.Count.Should().Be(3);
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
