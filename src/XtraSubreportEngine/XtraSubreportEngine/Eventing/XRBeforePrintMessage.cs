using System.Drawing.Printing;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine.Eventing
{
    public class XRBeforePrintMessage
    {
        public MyReportBase Report { get; private set; }
        public PrintEventArgs PrintArgs { get; private set; }

        public XRBeforePrintMessage(MyReportBase report, PrintEventArgs e)
        {
            Report = report;
            PrintArgs = e;
        }
    }
}