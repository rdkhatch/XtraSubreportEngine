using System;
using System.ComponentModel.Composition;

namespace XtraSubreport.Contracts.RuntimeActions
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ReportRuntimeActionExportAttribute : ExportAttribute, IReportRuntimeActionMetadata
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string GroupName { get; private set; }

        private static string _defaultGroupName = string.Empty;

        public ReportRuntimeActionExportAttribute(string uniqueActionName, string description)
            : this(uniqueActionName, _defaultGroupName, description)
        {
        }

        public ReportRuntimeActionExportAttribute(string uniqueActionName, string groupName, string description)
            : base(typeof(IReportRuntimeAction))
        {
            Name = uniqueActionName;
            GroupName = groupName;
            Description = description;
        }
    }

}
