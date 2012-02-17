using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.DesignTime;
using XtraSubreport.Design.Traversals;
using XtraSubreport.Engine.RuntimeActions;
using XtraSubreport.Engine.Support;

namespace XtraSubreport.Design
{

    public interface IDataSourceSetter
    {
        void SetReportDatasource(MyReportBase report, IReportDatasourceMetadata md);
        void SetReportDatasource(MyReportBase report, IReportDatasourceMetadata md, string traversalPath);
    }

    public interface IDataSourceTraverser
    {
        TraversedDatasourceResult TraversePath(object datasource, string path);
    }

    public interface IReportControllerFactory
    {
        IReportController GetController(XtraReport report);
    }

}
