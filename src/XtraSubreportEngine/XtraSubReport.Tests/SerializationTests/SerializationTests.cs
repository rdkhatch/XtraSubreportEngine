using System.Collections.Generic;
using System.Linq;
using NorthwindOData.Northwind;
using NUnit.Framework;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void can_serialize_custom_report_properties()
        {
            var report1 = new DummyReport();

            var abc = new DesignTimeDataSourceDefinition("a", "b", "c");
            report1.RyansSerializeTestObject = abc;

            var report2 = TestHelper.RunThroughSerializer(report1);

            Assert.IsNotNull(report2.RyansSerializeTestObject);
            Assert.AreEqual(report1.RyansSerializeTestObject, report2.RyansSerializeTestObject);
        }

        [Test]
        public void datasources_list_is_serialized()
        {
            var factory = new ReportFactory();

            var abc = new DesignTimeDataSourceDefinition("a", "b", "c");
            var xyz = new DesignTimeDataSourceDefinition("x", "y", "z");

            var report1 = (MyReportBase)factory.GetDefaultReport();

            // Collection
            report1.DesignTimeDataSources.Add(abc);
            report1.DesignTimeDataSources.Add(xyz);

            var report2 = TestHelper.RunThroughSerializer(report1);
            Assert.IsTrue(report2.DesignTimeDataSources.Count == 2);
        }

        [Test]
        public void datasource_should_resolves_types_when_setting_selected_datasource()
        {
            var designContext = TestHelper.CreateDesignerContext();
            var factory = new ReportFactory();

            var datasource1 = TestHelper.NorthwindDataSource;

            var report1 = (MyReportBase)factory.GetDefaultReport();

            // Set Selected Design-Time Datasource
            report1.ChangeDesignTimeDatasource(datasource1, designContext);

            Assert.AreEqual(datasource1.DataSourceType, typeof(List<Order>));
            Assert.AreEqual(datasource1.RootDataSourceType, typeof(List<Order>));
        }

        [Test]
        public void datasource_should_serialize_all_properties()
        {
            var datasource1 = new DesignTimeDataSourceDefinition("mydatasource", "mypath", "myrelation")
            {
                RootDataSourceType = typeof(Product),
                DataSourceType = typeof(Order)
            };

            var factory = new ReportFactory();

            var report1 = (MyReportBase)factory.GetDefaultReport();
            report1.DesignTimeDataSources.Add(datasource1);

            var report2 = TestHelper.RunThroughSerializer(report1);
            var datasource2 = report2.DesignTimeDataSources.Single();

            // Assert all properties were serialized
            Assert.AreEqual(datasource1.DataSourceName, datasource2.DataSourceName);
            Assert.AreEqual(datasource1.DataSourceRelationPath, datasource2.DataSourceRelationPath);
            Assert.AreEqual(datasource1.DataSourceType, datasource2.DataSourceType);
            Assert.AreEqual(datasource1.RootDataSourceType, datasource2.RootDataSourceType);
        }

        public class DummyReport : XtraReportWithCustomPropertiesBase
        {
            public DesignTimeDataSourceDefinition RyansSerializeTestObject { get; set; }

            protected override void DeclareCustomProperties()
            {
                base.DeclareCustomObjectProperty(() => this.RyansSerializeTestObject);
            }
        }

    }
}
