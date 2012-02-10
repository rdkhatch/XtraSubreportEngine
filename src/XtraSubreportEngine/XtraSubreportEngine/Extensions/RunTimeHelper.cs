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

            foreach (var subreportContainer in subreports)
                PassDatasourceToSubreport(subreportContainer);
        }

        #region Helper Methods

        private static void PassDatasourceToSubreport(XRSubreport subreportContainer)
        {
            subreportContainer.BeforePrint += (sender, e) =>
            {
                var subreportContainer = (XRSubreport)sender;
                SetDataSourceOnSubreport(subreportContainer);
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

        public static int SetRootHashCodeOnSubreport(XRSubreport subreportContainer)
        {
            var myReportBase = subreportContainer.NavigateToMyReportBase();
            var hashcode = myReportBase.RootHashCode;

            if(hashcode == 0)
                throw new Exception("Report did not have a root hashcode.");

            var subreportAsMyReportbase = ConvertReportSourceToMyReportBaseIfNeeded(subreportContainer);

            if (subreportAsMyReportbase != null)
                subreportAsMyReportbase.RootHashCode = hashcode;              

            return hashcode;
        }

        private static MyReportBase ConvertReportSourceToMyReportBaseIfNeeded(XRSubreport subreportContainer)
        {
            var subreportAsMyReportbase = subreportContainer.ReportSource as MyReportBase;

            if (subreportAsMyReportbase == null)
            {
                subreportAsMyReportbase = subreportContainer.ReportSource.ConvertReportToMyReportBase();
                subreportContainer.ReportSource = subreportAsMyReportbase;
            }        

            return subreportAsMyReportbase;
        }


        //   #endregion
    }
}
