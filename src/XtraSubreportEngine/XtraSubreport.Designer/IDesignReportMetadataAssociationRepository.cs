using System.Collections.Generic;
using System.Linq;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Design
{
    public interface IDesignReportMetadataAssociationRepository
    {
        IEnumerable<IReportDatasourceMetadataWithTraversal> GetAssociationsForReport(MyReportBase report);
        IReportDatasourceMetadataWithTraversal GetCurrentAssociationForReport(MyReportBase report);
        void AssociateWithReport(IReportDatasourceMetadataWithTraversal definition, MyReportBase report);
        void AssociateWithReportAsCurrent(IReportDatasourceMetadataWithTraversal definition, MyReportBase report);
    }
}