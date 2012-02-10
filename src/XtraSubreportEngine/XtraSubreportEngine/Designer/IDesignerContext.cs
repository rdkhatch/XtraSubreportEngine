
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreportEngine;
namespace XtraSubreport.Engine.Designer
{
    public interface IDesignerContext
    {
        DataSourceLocator DataSourceLocator { get; }
        XRDesignForm DesignForm { get; }
        IReportController GetController(XtraReport report);
    }

    internal interface IDesignerContextInternal
    {
        SubreportBase SelectedSubreport { get; set; }
    }
}
