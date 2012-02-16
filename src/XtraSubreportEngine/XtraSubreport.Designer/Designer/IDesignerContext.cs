
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Designer;
using XtraSubreport.Engine.RuntimeActions;
namespace XtraSubreport.Engine.Designer
{
    public interface IReportControllerFactory
    {
        IReportController GetController(XtraReport report);
    }

    public interface IDesignerContext
    {
        IDataSourceLocator DataSourceLocator { get; }
        XRDesignForm DesignForm { get; }
        string ProjectRootPath { get; }
    }

    internal interface IDesignerContextInternal
    {
        SubreportBase SelectedSubreport { get; set; }
    }
}
