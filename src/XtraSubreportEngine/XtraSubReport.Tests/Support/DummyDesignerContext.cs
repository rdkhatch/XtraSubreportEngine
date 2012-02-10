using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Engine.Designer;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreportEngine;

namespace XtraSubReport.Tests
{
    public class DummyDesignerContext : IDesignerContext
    {
        public DataSourceLocator DataSourceLocator { get; private set; }
        public XRDesignForm DesignForm { get; private set; }

        public IReportController GetController(XtraReport report)
        {
            throw new System.NotImplementedException("When is this called?");
        }

        public DummyDesignerContext()
        {
            DataSourceLocator = TestHelper.CreateDataSourceLocator();
            DesignForm = new XRDesignForm();
        }
    }
}
