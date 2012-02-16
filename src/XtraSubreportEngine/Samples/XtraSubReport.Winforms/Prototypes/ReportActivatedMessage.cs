using System.Linq;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms.Prototypes
{
    public class ReportActivatedMessage
    {
        public MyReportBase MyReport { get; set; }

        public ReportActivatedMessage(MyReportBase myReport)
        {
            MyReport = myReport;
        }
    }
}