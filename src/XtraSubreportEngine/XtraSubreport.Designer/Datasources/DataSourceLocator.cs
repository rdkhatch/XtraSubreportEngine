using System;
using System.Collections.Generic;
using System.Linq;
using XtraSubreport.Contracts.DesignTime;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Designer
{
    public class DataSourceLocator : IDataSourceLocator
    {
        private readonly List<IReportDatasourceProvider> _providers;

        public DataSourceLocator(List<IReportDatasourceProvider> providers)
        {
            _providers = providers;
        }

        public IEnumerable<IReportDatasourceProvider> GetReportDatasourceProviders()
        {
            return _providers;
        }

        public TraversedDatasourceResult GetDataSource(DesignTimeDataSourceDefinition definition)
        {
            var rootDatasource = GetDataSourceWithoutTraversal(definition);

            // Traverse Relation Path
            var targetDataSource = ObjectGraphPathTraverser.TraversePath(rootDatasource, definition.DataSourceRelationPath);

            // Assign Datasource Types to DesignTimeDatasource, not that we've obtained the datasource & traversed the relation path
            definition.RootDataSourceType = null;
            definition.DataSourceType = null;

            if (rootDatasource != null) definition.RootDataSourceType = rootDatasource.GetType();
            if (targetDataSource != null) definition.DataSourceType = targetDataSource.GetType();

            return new TraversedDatasourceResult(definition, rootDatasource, targetDataSource);
        }

        private object GetDataSourceWithoutTraversal(DesignTimeDataSourceDefinition definition)
        {
            var matches = (from provider in this.GetReportDatasourceProviders()
                           from datasourceMetadata in provider.GetReportDatasources()
                           where datasourceMetadata.UniqueId == definition.DataSourceUniqueId
                           select new { provider, datasourceMetadata }).ToList();

            if (matches.Count == 0)
                return null;

            if (matches.Count > 1)
                throw new Exception("Multiple Design-Time Datasources were found with UniqueId: {0}".FormatString(definition.DataSourceUniqueId));

            var match = matches.Single();

            // Fetch
            var datasource = match.provider.GetReportDatasource(match.datasourceMetadata.UniqueId);

            return datasource;
        }

    }



}