using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Engine.Designer;
using XtraSubreportEngine;

namespace XtraSubReport.Tests
{
    public class DummyDesignerContext : IDesignerContext
    {
        public DataSourceLocator DataSourceLocator { get; private set; }
        public XRDesignForm DesignForm { get; private set; }

        public DummyDesignerContext()
        {
            DataSourceLocator = TestHelper.CreateDataSourceLocator();
            DesignForm = new XRDesignForm();
        }
    }
}
