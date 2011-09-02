using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;

namespace XtraSubreport.Contracts.DataSources
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)] 
    public class ReportDatasourceExportAttribute : ExportAttribute, IReportDatasourceMetadata
    {
        public string Name { get; set; }
        public Type DataSourceType { get; set; }
        public string Description { get; set; }

        public ReportDatasourceExportAttribute(string datasourceName, string description)
            : this (datasourceName, description, typeof(object))
        {
        }

        public ReportDatasourceExportAttribute(string datasourceName, string description, Type datasourceType)
            : base(typeof(IReportDatasource))
        {
            Name = datasourceName;
            Description = description;
            DataSourceType = datasourceType;
        }

    }

}
