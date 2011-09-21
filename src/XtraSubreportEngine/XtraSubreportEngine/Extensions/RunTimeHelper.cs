using DevExpress.XtraReports.UI;

namespace XtraSubreport.Engine
{

    public static class RunTimeHelper
    {
        public static void PassDatasourceToSubreports(XtraReport report)
        {
            var subreports = report.FindAllSubreports();

            foreach (var subreport in subreports)
                PassDatasourceToSubreport(subreport);
        }

        #region Helper Methods

        private static void PassDatasourceToSubreport(XRSubreport subreportContainer)
        {
            subreportContainer.BeforePrint += (sender, e) =>
            {
                var subreport = (XRSubreport)sender;
                SetDataSourceOnSubreport(subreport);
            };
        }

        public static void SetDataSourceOnSubreport(XRSubreport subreport)
        {
            var datasource = subreport.Band.GetDataSource();

            // Good code below!
            if (datasource != null)
            {
                var report = subreport.ReportSource;

                report.SetReportDataSource(datasource);
            }
        }

        #endregion

    }
}
