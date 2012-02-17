using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine.Support
{
    /// <summary>
    /// Factory for MyReportBase
    /// </summary>
    public class ReportFactory : ReportTypeService
    {
        int reportCount = 0;

        public XtraReport GetDefaultReport()
        {
            reportCount += 1;
            var reportName = "Report{0}".FormatString(reportCount);
            
            var report = CreateReport(reportName);
            return report;
        }

        public MyReportBase GetNewReport()
        {
            return GetNewReport(string.Empty);
        }

        public MyReportBase GetNewReport(string reportName)
        {
            return CreateReport(reportName);
        }

        public Type GetType(Type reportType)
        {
            return reportType;
        }


        #region Ryan's Custom Methods

        private static MyReportBase CreateReport(string reportName)
        {
            var report = new MyReportBase();

            report.Name = reportName;
            report.DisplayName = reportName;

            var Detail = new DetailBand();
            var TopMargin = new TopMarginBand();
            var BottomMargin = new BottomMarginBand();

            report.Bands.AddRange(new DevExpress.XtraReports.UI.Band[]
            {
                Detail,
                TopMargin,
                BottomMargin
            });

            return report;
        }

        #endregion

    }


}
