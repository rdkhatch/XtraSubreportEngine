using System.Linq;
using XtraSubreport.Engine.Support;
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