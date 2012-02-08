using System.Drawing.Printing;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Engine.Eventing
{
    public class ScopedXRControlBeforePrintMessage
    {
        public int RootReportHashcode { get; private set; }
        public XRControl Control { get; private set; }

        public ScopedXRControlBeforePrintMessage(int rootReportHashcode, XRControl control)
        {
            RootReportHashcode = rootReportHashcode;
            Control = control;
        }
    }
}