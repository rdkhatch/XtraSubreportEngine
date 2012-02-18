using System.Collections.Generic;
using System.Linq;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Design.Traversals;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Design
{
    public interface IDesignDataRepository
    {
        IEnumerable<IReportDatasourceMetadata> GetAvailableMetadatas();
        object GetDataSourceByUniqueId(string uniqueId);
        IReportDatasourceMetadata GetDataSourceMetadataByUniqueId(string uniqueId);
    }

    
}