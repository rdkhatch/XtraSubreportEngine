using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindOData.Northwind;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void can_serialize_custom_report_properties()
        {
            var report1 = new DummyReport();

            var abc = new DesignTimeDataSourceDefinition("a", "b", "c");
            report1.RyansSerializeTestObject = abc;

            var report2 = TestHelper.RunThroughSerializer(report1);

            Assert.IsNotNull(report2.RyansSerializeTestObject);
            Assert.AreEqual(report1.RyansSerializeTestObject, report2.RyansSerializeTestObject);
        }

        [TestMethod]
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

        [TestMethod]
        public void datasource_should_resolves_types_when_setting_selected_datasource()
        {
            var factory = new ReportFactory();

            var datasource1 = TestHelper.NorthwindDataSource;

            var report1 = (MyReportBase)factory.GetDefaultReport();

            // Set Selected Design-Time Datasource
            report1.SelectedDesignTimeDataSource = datasource1;

            var report2 = TestHelper.RunThroughSerializer(report1);
            var datasource2 = report2.DesignTimeDataSources.Single();

            Assert.IsNotNull(datasource2);
            Assert.AreEqual(datasource1.DataSourceType, typeof(List<Order>));
            Assert.AreEqual(datasource1.RootDataSourceType, typeof(List<Order>));

        }

        [TestMethod]
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
            Assert.AreEqual(datasource1.DataSourceAssemblyLocationPath, datasource2.DataSourceAssemblyLocationPath);
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
