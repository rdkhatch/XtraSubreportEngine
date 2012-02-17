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
    [TestFixture]
    public class Message_action_handler_tests
    {

        public class TestReportDatasourceMetadata : IReportDatasourceMetadata
        {
            public TestReportDatasourceMetadata(string uniqueId)
            {
                UniqueId = uniqueId;
            }

            public string UniqueId { get; private set; }

            public string Name
            {
                get { return "Person2"; }
            }

            public string Description
            {
                get { return "This is some people"; }
            }

            public Type DataSourceType
            {
                get { return typeof (Person2); }
            }
        }

        public class TestReportDatasourceProvider : IReportDatasourceProvider
        {
            public IEnumerable<IReportDatasourceMetadata> GetReportDatasources()
            {
                return new [] {new TestReportDatasourceMetadata("DogTime")};
            }
            
            public object GetReportDatasource(string uniqueId)
            {
                return Person2.SampleData();
            }
        }

        public class TestDesignDataContext : IDesignDataContext
        {
            public TestDesignDataContext(IEnumerable<IReportDatasourceProvider> providers, IDesignDataRepository designDataRepository, IDesignReportMetadataAssociationRepository designReportMetadataAssociationRepository)
            {
                Providers = providers;
                DesignDataRepository = designDataRepository;
                DesignReportMetadataAssociationRepository = designReportMetadataAssociationRepository;
            }

            public IEnumerable<IReportDatasourceProvider> Providers { get; private set; }
            public IDesignDataRepository DesignDataRepository { get; private set; }

            public IDesignReportMetadataAssociationRepository DesignDataDefinitionRepository
            {
                get { throw new NotImplementedException(); }
            }

            public IDesignReportMetadataAssociationRepository DesignReportMetadataAssociationRepository { get; private set; }
        }
        
        [Test]
        public void Should_do_everything()
        {
            // given infrastructure
            var providers = new List<IReportDatasourceProvider> {new TestReportDatasourceProvider()};
            var dataDefRep = new DesignReportMetadataAssociationRepository();
            IDesignDataRepository datarep = new DesignDataRepository(providers);
            var setter = new DataSourceSetter(datarep, dataDefRep, new ObjectGraphPathTraverser());
            var handler = new ActionMessageHandler( setter, new EventAggregator(), datarep,dataDefRep, new ReportControllerFactory());
            
            // given a report
            var report = new XtraReportWithSubReportInDetailReport();
            var report2 = report.CloneLayoutAsMyReportBase();

            

            // given the parent has a datasource
            IReportDatasourceMetadata metadata = datarep.GetAvailableMetadatas().Single(a => a.Name == "DogTime");
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




        [Test]
        public void Test()
        {
            IDataSourceSetter unknown = null;
            IReportDatasourceMetadata md = null;
            var report = new MyReportBase();
            unknown.SetReportDatasource(report, md);
            report.DataSource.Should().NotBeNull();
        }

        [Test]
        public void Test2()
        {
            IDataSourceSetter unknown = null;
            IReportDatasourceMetadata md = null;
            var report = new MyReportBase();
            unknown.SetReportDatasource(report, md, "Dogs");
            report.DataSource.Should().NotBeNull();
        }

    }


}