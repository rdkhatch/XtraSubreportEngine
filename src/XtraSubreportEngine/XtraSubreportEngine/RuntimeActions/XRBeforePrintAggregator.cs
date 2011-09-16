using System;
using System.Drawing.Printing;
using DevExpress.XtraReports.UI;

namespace XtraSubreport.Engine.RuntimeActions
{
    public static class XRBeforePrintAggregator
    {
        /// <summary>
        /// Reports pass themselves into the Aggregator at runtime
        /// </summary>
        /// <param name="report"></param>
        /// <param name="e"></param>
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
