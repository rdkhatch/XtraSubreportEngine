using System.Linq;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Design;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReport.Tests.Support
{
    public class DummyDesignerContext// : IDesignerContext
    {
        public IDesignDataRepository Design_data_repository { get; private set; }
        public XRDesignForm DesignForm { get; private set; }

        public IReportController GetController(XtraReport report)
        {
            throw new System.NotImplementedException("When is this called?");
        }

        public string ProjectRootPath
        {
            get { throw new System.NotImplementedException(); }
        }

        public DummyDesignerContext(IDesignDataRepository locator)
        {
            Design_data_repository = locator;
            DesignForm = new XRDesignForm();
        }
    }
}
