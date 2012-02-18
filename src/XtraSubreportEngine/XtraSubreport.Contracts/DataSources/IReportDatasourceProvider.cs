using System.Linq;
using System.Collections.Generic;

namespace XtraSubreport.Contracts.DataSources
{
    public interface IReportDatasourceProvider
    {
        IEnumerable<IReportDatasourceMetadata> GetReportDatasources();
        object GetReportDatasource(string uniqueId);
    }
}
