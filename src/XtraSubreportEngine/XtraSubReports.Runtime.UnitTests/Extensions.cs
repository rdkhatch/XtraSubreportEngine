using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreportEngine.Support;

namespace XtraSubReports.Runtime.UnitTests
{
    public static class Extensions
    {
        public static void ExportToMemory(this XtraReport report)
        {
            report.ExportToHtml(new MemoryStream());
        }

        public static MyReportBase CloneUsingLayout(this XtraReport report)
        {
            var stream = new MemoryStream();
            report.SaveLayout(stream);
            stream.Position = 0;

            var newReport = new MyReportBase();
            newReport.LoadLayout(stream);
            newReport.DataSource = report.DataSource;
            return newReport;
        }
    }
}