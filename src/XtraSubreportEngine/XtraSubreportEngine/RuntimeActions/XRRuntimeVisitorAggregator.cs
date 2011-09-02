using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using System.Drawing.Printing;

namespace XtraSubreport.Engine.RuntimeActions
{
    public static class XRRuntimeVisitorAggregator
    {
        public static void FireBeforePrintEvent(XtraReport report, PrintEventArgs e)
        {
            if (BeforePrint != null)
                BeforePrint.Invoke(report, new BeforePrintEventArgs(report, e));
        }

        public static event EventHandler<BeforePrintEventArgs> BeforePrint;

    }

    public class BeforePrintEventArgs : EventArgs
    {
        PrintEventArgs _args;

        public BeforePrintEventArgs(XtraReport report, PrintEventArgs e)
        {
            Report = report;
            _args = e;
        }

        public XtraReport Report { get; private set; }

        // Proxies value to/from the real PrintArgs that we were given
        public bool Cancel
        {
            get { return _args.Cancel; }
            set { _args.Cancel = value; }
        }
    }
}
