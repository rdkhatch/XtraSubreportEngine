using System.Collections.Generic;

namespace XtraSubreport.Contracts.DesignTime
{
    public interface IReportDatasourceProvider
    {
        IEnumerable<IReportDatasourceMetadata> GetReportDatasources();
        object GetReportDatasource(string uniqueId);
    }
}
