using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using NorthwindOData;
using XtraSubReport.Winforms.Repositories;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Design;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreport.Engine.Support;

namespace XtraSubReport.Tests.Support
{
    static class TestHelper
    {

        public static IDesignDataRepository CreateDataSourceRepository()
        {
            var providers = new List<IReportDatasourceProvider>() { new NorthwindDatasourceProvider() };
            return new DesignDataRepository(providers);
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

        public static ReportDatasourceMetadataWithTraversal NorthwindDataSource
        {
            get { return new ReportDatasourceMetadataWithTraversal(new NorthwindOrders(), "Northwind_Orders", typeof(NorthwindOrders)); }
        }

        public static object GetNorthwindOrders()
        {
            var locator = CreateDataSourceRepository();
            return locator.GetDataSourceByUniqueId(NorthwindDataSource.UniqueId);
        }

        public static Tuple<MyReportBase, XRSubreport, MyReportBase, object> GetParentAndNestedSubreport()
        {
            throw new NotImplementedException("Method does not compile");
/*            var factory = new ReportFactory();

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
            var designContext = TestHelper.CreateDesignerContext();
            parentReport.ChangeDesignTimeDatasource(TestHelper.NorthwindDataSource, designContext);
            Assert.IsNotNull(parentReport.DataSource);

            return new Tuple<MyReportBase, XRSubreport, MyReportBase, IDesignerContext>(parentReport, subreportContainer, subreport, designContext);*/
        }

        public static void RunReport(XtraReport report, params IReportRuntimeAction[] action)
        {
            var controller = new XRReportController(report, new XRRuntimeActionFacade(action));
            controller.Print(r => r.ExportToHtml(new MemoryStream(), new HtmlExportOptions()));
        }


    }
}
