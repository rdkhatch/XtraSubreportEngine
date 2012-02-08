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
    }
}