using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms.Prototypes
{
    public class ReportActivatedBySubreportMessage
    {
        public MyReportBase MyReport { get; set; }
        public SubreportBase SelectedSubreport { get; set; }

        public ReportActivatedBySubreportMessage(MyReportBase myReport, SubreportBase selectedSubreport)
        {
            MyReport = myReport;
            SelectedSubreport = selectedSubreport;
        }
    }
}