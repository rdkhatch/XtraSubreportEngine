using System;
using System.IO;
using DevExpress.XtraReports.UI;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine
{

    public static class RunTimeHelper
    {
/*
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
*/

        public static object SetDataSourceOnSubreport(XRSubreport subreport)
        {
            var datasource = subreport.Band.GetDataSource();

            // Good code below!
            if (datasource != null)
            {
                var report = subreport.ReportSource;

                report.SetReportDataSource(datasource);
            }

            return datasource;
        }

        public static int SetRootHashCodeOnSubreport(XRSubreport subreport)
        {
            var hashcode = ((MyReportBase)subreport.Report).RootHashCode;

            if(hashcode == 0)
                throw new Exception("Report did not have a root hashcode.");

            var report = (MyReportBase)subreport.ReportSource ;
            report.RootHashCode = hashcode;              

            return hashcode;
        }



     //   #endregion
    }
}
