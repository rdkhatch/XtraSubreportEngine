using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Design;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReport.Winforms.Support
{
    public class ReportControllerFactory : IReportControllerFactory
    {
        private readonly XRRuntimeActionFacade _xrRuntimeActionFacade;

        public ReportControllerFactory() : this(null)
        {
        }

        public ReportControllerFactory(XRRuntimeActionFacade xrRuntimeActionFacade)
        {
            _xrRuntimeActionFacade = xrRuntimeActionFacade;
        }

        public IReportController GetController(XtraReport report)
        {
            return new XRReportController(report,_xrRuntimeActionFacade);
        }
    }
}