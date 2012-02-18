using System.Collections.Generic;
using System.Linq;
using XtraSubreport.Contracts.DataSources;

namespace NorthwindOData
{
    public class NorthwindDatasourceProvider : IReportDatasourceProvider
    {
        private readonly List<IReportDatasourceMetadata> datasources = new List<IReportDatasourceMetadata>();

        public NorthwindDatasourceProvider()
        {
            datasources.Add(new NorthwindOrders());
        }

        public IEnumerable<IReportDatasourceMetadata> GetReportDatasources()
        {
            return datasources;
        }

        public object GetReportDatasource(string uniqueId)
        {
            var export = datasources.Where(d => d.UniqueId == uniqueId).SingleOrDefault();
            var fetcher = export as IFetchData;
            return fetcher.Fetch();
        }

        internal interface IFetchData
        {
            object Fetch();
        }
    }
}