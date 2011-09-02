using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreport.Engine;
using XtraSubreportEngine.Support;
using System.IO;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Serialization;
using XtraSubreport.Engine.Support;
using DevExpress.XtraPrinting;
using XtraSubreport.Engine.Extensions;
using System.Collections;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class ReportTests
    {
        [TestMethod]
        public void service_creates_custom_report_class()
        {
            var factory = new ReportFactory();
            var report = factory.GetDefaultReport();

            Assert.IsTrue(report is MyReportBase);
        }

        [TestMethod]
        public void populates_design_time_datasource()
        {
            var form = TestHelper.CreateDesignForm();
            var factory = new ReportFactory();

            // Parent Report
            var parentReport = factory.GetNewReport();
            var parentDataSource = TestHelper.NorthwindDataSource;
            parentReport.DesignTimeDataSources.Add(parentDataSource);

            DesignTimeHelper.PopulateDesignTimeDataSource(parentReport);

            Assert.IsNotNull(parentReport.DataSource);
        }

        [TestMethod]
        public void save_parent_and_subreport_for_opening_in_designer()
        {
            var tuple = TestHelper.GetParentAndNestedSubreport();

            var parentReport = tuple.Item1;
            var subreportContainer = tuple.Item2;
            var subreport = tuple.Item3;

            var parentPath = @"C:\temp\reports\parent.repx";
            var subreportPath = @"C:\temp\reports\subreport.repx";

            subreportContainer.ReportSource = null;
            subreportContainer.ReportSourceUrl = subreportPath;
            subreport.SelectedDesignTimeDataSource = TestHelper.NorthwindDataSource;
            subreport.SelectedDesignTimeDataSource.DataSourceRelationPath = "[0].Order_Details";
            parentReport.SaveLayout(parentPath);
            subreport.SaveLayout(subreportPath);
        }


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
