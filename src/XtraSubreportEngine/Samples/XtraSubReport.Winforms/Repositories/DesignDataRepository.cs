using System;
using System.Collections.Generic;
using System.Linq;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Design;
using XtraSubreport.Design.Traversals;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms.Repositories
{
    public class DesignDataRepository : IDesignDataRepository
    {
        private readonly IEnumerable<IReportDatasourceProvider> _providers;

        public DesignDataRepository(IEnumerable<IReportDatasourceProvider> providers)
        {
            _providers = providers;
        }

        IEnumerable<IReportDatasourceMetadata> IDesignDataRepository.GetAvailableMetadatas()
        {
            var items = from p in _providers
                        from md in p.GetReportDatasources()
                        select md;

            return items;
        }

        object IDesignDataRepository.GetDataSourceByUniqueId(string uniqueId)
        {
            var tuple = FetchAvailableProvidersAndMetadatas(uniqueId);
            return tuple.Item1.GetReportDatasource(tuple.Item2.UniqueId);
        }

        public IReportDatasourceMetadata GetDataSourceMetadataByUniqueId(string uniqueId)
        {
            return FetchAvailableProvidersAndMetadatas(uniqueId).Item2;
        }

        private Tuple<IReportDatasourceProvider, IReportDatasourceMetadata> FetchAvailableProvidersAndMetadatas(string uniqueId)
        {
            var matches = (from provider in _providers
                           from datasourceMetadata in provider.GetReportDatasources()
                           where datasourceMetadata.UniqueId == uniqueId
                           select new Tuple<IReportDatasourceProvider,IReportDatasourceMetadata>(provider, datasourceMetadata)).ToList();

            if (matches.Count == 0)
                return null;

            if (matches.Count > 1)
                throw new Exception("Multiple Design-Time Datasources were found with UniqueId: {0}".FormatString(uniqueId));

            var match = matches.Single();
            return match;
        }
    }
}