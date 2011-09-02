using System;

namespace XtraSubreport.Contracts.DataSources
{
    // Interface for MEF metadata
    public interface IReportDatasourceMetadata
    {
        string Name { get; }
        Type DataSourceType { get; }
        string Description { get; }
    }
}
