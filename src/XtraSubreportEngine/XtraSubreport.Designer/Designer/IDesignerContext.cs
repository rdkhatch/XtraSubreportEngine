
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Designer;
using XtraSubreport.Engine.RuntimeActions;
namespace XtraSubreport.Engine.Designer
{
    public interface IDesignerContext
    {
        IDataSourceLocator DataSourceLocator { get; }
        XRDesignForm DesignForm { get; }
        IReportController GetController(XtraReport report);
        string ProjectRootPath { get; }
    }

    internal interface IDesignerContextInternal
    {
        SubreportBase SelectedSubreport { get; set; }
    }
}
