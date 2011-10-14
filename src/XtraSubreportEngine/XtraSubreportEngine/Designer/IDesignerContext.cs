
using DevExpress.XtraReports.UserDesigner;
using XtraSubreportEngine;
namespace XtraSubreport.Engine.Designer
{
    public interface IDesignerContext
    {
        DataSourceLocator DataSourceLocator { get; }
        XRDesignForm DesignForm { get; }
    }
}
