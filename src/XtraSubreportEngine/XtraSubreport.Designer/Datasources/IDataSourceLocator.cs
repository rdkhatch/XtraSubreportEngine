using System.Collections.Generic;
using XtraSubreport.Contracts.DesignTime;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Designer
{
    public interface IDataSourceLocator
    {
        IEnumerable<IReportDatasourceProvider> GetReportDatasourceProviders();

        TraversedDatasourceResult GetDataSource(DesignTimeDataSourceDefinition definition);
    }
}