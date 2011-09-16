
namespace XtraSubreport.Contracts.RuntimeActions
{
    // Interface for MEF metadata
    public interface IReportRuntimeActionMetadata
    {
        string Name { get; }
        string Description { get; }
        string GroupName { get; }
    }
}
