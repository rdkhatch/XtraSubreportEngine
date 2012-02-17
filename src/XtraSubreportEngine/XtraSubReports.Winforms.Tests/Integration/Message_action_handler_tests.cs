using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using NUnit.Framework;
using XtraSubReport.Winforms.Prototypes;
using XtraSubReport.Winforms.Repositories;
using XtraSubReport.Winforms.Support;
using XtraSubReports.TestResources.Infrastructure;
using XtraSubReports.TestResources.Models;
using XtraSubReports.TestResources.Reports;
using XtraSubreport.Contracts.DesignTime;
using XtraSubreport.Design;
using XtraSubreport.Design.Traversals;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Eventing;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Winforms.Tests.Integration
{


/*
    [TestFixture]
    public class When_creating_a_person : SpecificationBase
    {
        private static DesignReportMetadataAssociationRepository init(out DataSourceSetter setter,
                                                                  out IDesignDataRepository datarep)
        {
            var providers = new List<IReportDatasourceProvider> { new DogTimeReportDatasourceProvider() };
            var dataDefRep = new DesignReportMetadataAssociationRepository();
            datarep = new DesignDataRepository(providers);
            setter = new DataSourceSetter(datarep, dataDefRep, new ObjectGraphPathTraverser());
            return dataDefRep;
        }

        DataSourceSetter _setter;
        IDesignDataRepository _datarep;
        DesignReportMetadataAssociationRepository _dataDefRep;

        protected override void Given()
        {
            _dataDefRep = init(out _setter, out _datarep);
        }

        protected override void When()
        {
            _p = new Person();
        }

        [Then]
        public void object_should_not_be_null()
        {
            _p.Should().NotBeNull("Person should not be null");
        }
    }*/


    [TestFixture]
    public class Setter_tests
    {


        
        [Test]
        public void Should_do_everything()
        {
            //TODO: rewrite as spec or individual unit tests!

            // given infrastructure
            DataSourceSetter setter;
            IDesignDataRepository datarep;
            var dataDefRep = init(out setter, out datarep);
            var handler = new ActionMessageHandler( setter, new EventAggregator(), datarep,dataDefRep, new ReportControllerFactory());
            
            // given a report
            var report = new XtraReportWithSubReportInDetailReport();
            var report2 = report.CloneLayoutAsMyReportBase();

            

            // given the parent has a datasource
            IReportDatasourceMetadata metadata = datarep.GetAvailableMetadatas().Single(a => a.UniqueId == "DogTime");
            setter.SetReportDatasource(report2, metadata);
            
            // given a subreport in parent report
            var newSubReport = new MyReportBase();
            var band = (DetailReportBand) report2.Bands[BandKind.DetailReport];
            var myContainer = (XRSubreport) band.Bands[BandKind.Detail].Controls[0];
            
            
            // when handling a message
            var message = new ReportActivatedBySubreportMessage(newSubReport, myContainer);
            handler.Handle(message);

            // then:
            newSubReport.DataSource.Should().NotBeNull();
            var dog = (Dog)((List<object>) newSubReport.DataSource).Single();
            var peoples = (List<Person2>) report2.DataSource;

            peoples[0].Dogs[0].Name.Should().Be(dog.Name);
        }

        private static DesignReportMetadataAssociationRepository init(out DataSourceSetter setter,
                                                                          out IDesignDataRepository datarep)
        {
            var providers = new List<IReportDatasourceProvider> {new DogTimeReportDatasourceProvider()};
            var dataDefRep = new DesignReportMetadataAssociationRepository();
            datarep = new DesignDataRepository(providers);
            setter = new DataSourceSetter(datarep, dataDefRep, new ObjectGraphPathTraverser());
            return dataDefRep;
        }


        [Test]
        public void Should_set_datasource_on_nontraversal()
        {
            DataSourceSetter setter;
            IDesignDataRepository datarep;
            DesignReportMetadataAssociationRepository dataDefRep = init(out setter, out datarep);

            var md = datarep.GetDataSourceMetadataByUniqueId("DogTime");
            var report = new MyReportBase();
            setter.SetReportDatasource(report, md);
            report.DataSource.Should().NotBeNull();
            var persons = ((List<Person2>)report.DataSource);

            persons.Count().Should().Be(3);
        }

        [Test]
        public void Should_set_datasource_on_traversal()
        {
            DataSourceSetter setter;
            IDesignDataRepository datarep;
            var dataDefRep = init(out setter, out datarep);

            var md = datarep.GetDataSourceMetadataByUniqueId("DogTime");
            var report = new MyReportBase();
            setter.SetReportDatasource(report, md,"Dogs");
            report.DataSource.Should().NotBeNull();
            var dogs = ((List<Dog>) report.DataSource);

            dogs.Count().Should().Be(2);
        }

    }


}