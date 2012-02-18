using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using TechTalk.SpecFlow;
using XtraSubReport.Winforms.Prototypes;
using XtraSubReport.Winforms.Repositories;
using XtraSubReport.Winforms.Support;
using XtraSubReports.TestResources.Infrastructure;
using XtraSubReports.TestResources.Models;
using XtraSubReports.TestResources.Reports;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Design;
using XtraSubreport.Design.Traversals;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Eventing;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreport.Engine.Support;

namespace XtraSubReport.Winforms.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Passing datasources at design time")]
   public class Passing_datasources_at_design_time
    {
        IDataSourceSetter _setter;
        IDesignDataContext _dataContext;
        private IReportDatasourceMetadata _datasourceMetadata;
        private MyReportBase _parentReport;
        private MyReportBase _newReport;
        private ActionMessageHandler _messageHandler;
        private XRSubreport _container;
        private IEventAggregator _eventAggregator;
        [Given(@"The design runtime is ready")]
        public void GivenTheDesignRuntimeIsReady()
        {
            _eventAggregator = new EventAggregator();
            _dataContext = Factory.CreateForDogTime(out _setter);
            _messageHandler = new ActionMessageHandler(_setter,_eventAggregator,_dataContext.DesignDataRepository,_dataContext.DesignDataDefinitionRepository,new ReportControllerFactory());
        }

        [Given(@"a datasource exists called DogTime")]
        public void GivenADatasourceExistsCalledDogTime()
        {
            _datasourceMetadata = _dataContext.DesignDataRepository.GetDataSourceMetadataByUniqueId("DogTime");
        }

        [Given(@"PersonReport exists with a subreport called DogReport in a detail report")]
        public void GivenPersonReportExistsWithASubreportCalledDogReportInADetailReport()
        {
            _parentReport = new XtraReportWithSubReportInDetailReport().ConvertReportToMyReportBase();
           
            var band = (DetailReportBand)_parentReport.Bands[BandKind.DetailReport];
            _container = (XRSubreport)band.Bands[BandKind.Detail].Controls[0];
        }

        
        [Given(@"PersonReport exists with a subreport called PersonSubReport in a detail band")]
        public void GivenPersonReportExistsWithASubreportCalledPersonSubReportInADetailBand()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"PersonReport loads the DogTime datasource")]
        public void PersonReportLoadsTheDogTimeDatasource()
        {
            _setter.SetReportDatasource(_parentReport,_datasourceMetadata);
        }

        [Given(@"a new report instance exists")]
        public void GivenANewReportInstanceExists()
        {
            _newReport = new MyReportBase();
        }

        [When(@"A ReportActivatedBySubreportMessage occurs which contains the new report instance")]
        public void WhenAReportActivatedBySubreportMessageOccursWhichContainsTheNewReportInstance()
        {
            _eventAggregator.Publish(new ReportActivatedBySubreportMessage(_newReport,_container));
        }


        [Then(@"the new report instance's datasource should be the first dog of the first person from PersonReport's datasource")]
        public void ThenTheNewReportInstanceSDatasourceShouldBeTheFirstDogOfTheFirstPersonFromPersonReportSDatasource()
        {
            var dog = (Dog)((List<object>)_newReport.DataSource).Single();
            var peoples = (List<Person2>)_parentReport.DataSource;

            peoples[0].Dogs[0].Name.Should().Be(dog.Name);
        }

        [Then(@"the new report instance's datasource should be the first person from the PersonReport's datasource")]
        public void ThenTheNewReportInstanceSDatasourceShouldBeTheFirstPersonFromThePersonReportSDatasource()
        {
            ScenarioContext.Current.Pending();
        }



    }
}
