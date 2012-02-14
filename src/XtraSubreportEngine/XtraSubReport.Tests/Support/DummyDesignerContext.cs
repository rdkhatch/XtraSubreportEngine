using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Designer;
using XtraSubreport.Engine.Designer;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReport.Tests
{
    public class DummyDesignerContext : IDesignerContext
    {
        public IDataSourceLocator DataSourceLocator { get; private set; }
        public XRDesignForm DesignForm { get; private set; }

        public IReportController GetController(XtraReport report)
        {
            throw new System.NotImplementedException("When is this called?");
        }

        public string ProjectRootPath
        {
            get { throw new System.NotImplementedException(); }
        }

        public DummyDesignerContext(IDataSourceLocator locator)
        {
            DataSourceLocator = locator;
            DesignForm = new XRDesignForm();
        }
    }
}
