using System.Linq;
using System;
using GeniusCode.Framework.Support.Collections.Tree;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms.Popups.Support
{
    public class DesignTimeDataSourceTreeItem : IDynamicTreeviewItem
    {
        public ReportDatasourceMetadataWithTraversal DesignTimeDataSourceDefinition { get; set; }
        public IReportDatasourceMetadata Metadata { get; set; }
        public string PreviouslyUsedWithThisReport { get; set; }
        public string RelationPath { get; set; }

        public Type DataSourceType
        {
            get
            {
                if (IsDatasourceAvailable)
                    return Metadata.DataSourceType;

                return null;
            }
        }


        public string Description
        {
            get
            {
                if (IsDatasourceAvailable)
                    return Metadata.Description;

                return string.Empty;
            }
        }

        public bool IsDatasourceAvailable { get { return Metadata != null; } }

        #region IDyanmicTreeView Implementation

        public string Name { get; set; }

        public int ID { get; set; }

        public int ImageIndex { get; set; }

        public bool IsStructure { get; set; }

        public int ParentID { get; set; }

        public string Path { get; set; }

        #endregion
    }
}
