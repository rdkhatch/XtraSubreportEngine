using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;

namespace XtraSubreport.Contracts.RuntimeActions
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)] 
    public class ReportRuntimeActionExportAttribute : ExportAttribute, IReportRuntimeActionMetadata
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ReportRuntimeActionExportAttribute(string datasourceName, string description)
            : base(typeof(IReportRuntimeAction))
        {
            Name = datasourceName;
            Description = description;
        }
    }

}
