using System;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreport.Engine;
using XtraSubreportEngine;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Tests
{
    static class TestHelper
    {
        public static XRDesignForm CreateDesignForm()
        {
            return new XRDesignForm();
        }

        public static TReport RunThroughSerializer<TReport>(TReport report)
            where TReport : XtraReport
        {
            var stream = new MemoryStream();

            report.SaveLayout(stream);

            stream.Position = 0;

            var report2 = XtraReport.FromStream(stream, true);

            return (TReport)report2;
        }

        public static DesignTimeDataSourceDefinition NorthwindDataSource
        {
            get { return new DesignTimeDataSourceDefinition("Northwind_Products", string.Empty, string.Empty); }
        }

        public static object GetNorthwindProducts()
        {
            return DataSourceLocator.GetDatasource(NorthwindDataSource).Value.DataSource;
        }

        public static Tuple<MyReportBase, XRSubreport, MyReportBase> GetParentAndNestedSubreport()
        {
            var factory = new ReportFactory();

            // Parent Report
            var parentReport = factory.GetNewReport("parentReport");

            // Subreport
            var subreport = factory.GetNewReport("subreport");

            // Order-Details Band
            var detailReport = new DetailReportBand() { DataMember = "Order_Details" };
            var detailBand = new DetailBand();
            parentReport.Bands.Add(detailReport);
            detailReport.Bands.Add(detailBand);

            // Insert Subreport into Subreport Container... for Runtime Testing
            var subreportContainer = new XRSubreport() { ReportSource = subreport };
            detailBand.Controls.Add(subreportContainer);

            // Datasource MUST be set AFTER the Detail Band has been added!!  Otherwise detailband gets empty object[] as its datasource
            parentReport.SelectedDesignTimeDataSource = TestHelper.NorthwindDataSource;
            Assert.IsNotNull(parentReport.DataSource);

            return new Tuple<MyReportBase, XRSubreport, MyReportBase>(parentReport, subreportContainer, subreport);
        }

        public static void RunReport(XtraReport report)
        {
            report.ExportToHtml(new MemoryStream(), new HtmlExportOptions());
        }
    }
}
