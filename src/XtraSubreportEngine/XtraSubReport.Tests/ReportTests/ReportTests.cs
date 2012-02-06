using NUnit.Framework;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Tests
{
    [TestFixture]
    public class ReportTests
    {
        [Test]
        public void service_creates_custom_report_class()
        {
            var factory = new ReportFactory();
            var report = factory.GetDefaultReport();

            Assert.IsTrue(report is MyReportBase);
        }

        [Test]
        public void populates_design_time_datasource()
        {
            var factory = new ReportFactory();
            var designContext = TestHelper.CreateDesignerContext();

            var report = factory.GetNewReport();
            var datasourceDefinition = TestHelper.NorthwindDataSource;
            report.DesignTimeDataSources.Add(datasourceDefinition);

            report.ChangeDesignTimeDatasourceToDefault(designContext);

            AssertHelper.Reports.AssertDatasourceHasItems(report);
        }

        [Test]
        public void save_parent_and_subreport_for_opening_in_designer()
        {
            var tuple = TestHelper.GetParentAndNestedSubreport();

            var parentReport = tuple.Item1;
            var subreportContainer = tuple.Item2;
            var subreport = tuple.Item3;
            var designContext = tuple.Item4;

            var parentPath = @"C:\temp\reports\parent.repx";
            var subreportPath = @"C:\temp\reports\subreport.repx";

            subreportContainer.ReportSource = null;
            subreportContainer.ReportSourceUrl = subreportPath;

            var datasourceDefinition = TestHelper.NorthwindDataSource;
            datasourceDefinition.DataSourceRelationPath = "[0].Order_Details";
            subreport.ChangeDesignTimeDatasource(datasourceDefinition, designContext);

            parentReport.SaveLayout(parentPath);
            subreport.SaveLayout(subreportPath);
        }

        [Test]
        public void should_not_throw_exception_when_no_design_time_datasources()
        {
            var factory = new ReportFactory();
            var designContext = TestHelper.CreateDesignerContext();

            var report = factory.GetNewReport();

            // Should not throw exception
            report.ChangeDesignTimeDatasourceToDefault(designContext);
        }

        [Test]
        public void should_replace_existing_datasource()
        {
            var factory = new ReportFactory();
            var designContext = TestHelper.CreateDesignerContext();

            var report = factory.GetNewReport();

            // Dummy datasource
            var dummy = new int[] { 1, 2, 3 };
            report.DataSource = dummy;

            report.ChangeDesignTimeDatasourceToDefault(designContext);

            Assert.AreEqual(null, report.DataSource);
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
