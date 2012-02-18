using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms.Prototypes
{
    public class ReportActivatedBySubreportMessage
    {
        public MyReportBase NewReport { get; set; }
        public SubreportBase SelectedSubreport { get; set; }

        public ReportActivatedBySubreportMessage(MyReportBase myReport, SubreportBase selectedSubreport)
        {
            NewReport = myReport;
            SelectedSubreport = selectedSubreport;
        }
    }
}