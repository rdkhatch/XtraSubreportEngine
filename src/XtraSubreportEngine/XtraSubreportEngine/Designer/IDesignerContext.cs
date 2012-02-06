
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreportEngine;
namespace XtraSubreport.Engine.Designer
{
    public interface IDesignerContext
    {
        DataSourceLocator DataSourceLocator { get; }
        XRDesignForm DesignForm { get; }
    }

    internal interface IDesignerContextInternal
    {
        SubreportBase SelectedSubreport { get; set; }
    }
}
