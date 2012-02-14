using System;

namespace XtraSubreport.Contracts.DesignTime
{
    public interface IReportDatasourceMetadata
    {
        string UniqueId { get; }
        string Name { get; }
        string Description { get; }
        Type DataSourceType { get; }
    }
}