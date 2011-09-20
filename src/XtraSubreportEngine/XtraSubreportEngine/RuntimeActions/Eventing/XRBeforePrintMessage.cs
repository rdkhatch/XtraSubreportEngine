using System.Drawing.Printing;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Engine
{
    public class XRBeforePrintMessage
    {
        public XtraReport Report { get; private set; }
        public PrintEventArgs PrintArgs { get; private set; }

        public XRBeforePrintMessage(XtraReport report, PrintEventArgs e)
        {
            Report = report;
            PrintArgs = e;
        }
    }
}