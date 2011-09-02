using System;
using System.Collections.Generic;
using System.Linq;
using GeniusCode.Framework.Support.Collections.Tree;
using XtraSubreportEngine.Support;
using XtraSubreport.Contracts.DataSources;

namespace XtraSubreport.Engine.Support
{
    public class DesignTimeDataSourceTreeItem : IDynamicTreeviewItem
    {
        public DesignTimeDataSourceDefinition DesignTimeDataSourceDefinition { get; set; }
        public IReportDatasourceMetadata MEFMetadata { get; set; }
        public string PreviouslyUsedWithThisReport { get; set; }
        public string RelationPath { get; set; }

        public Type DataSourceType
        {
            get
            {
                if (IsDatasourceAvailable)
                    return MEFMetadata.DataSourceType;

                return null;
            }
        }


        public string Description
        {
            get
            {
                if (IsDatasourceAvailable)
                    return MEFMetadata.Description;

                return string.Empty;
            }
        }

        public bool IsDatasourceAvailable { get { return MEFMetadata != null; } }

        #region IDyanmicTreeView Implementation

        public string Name { get; set; }

        public int ID { get; set;}

        public int ImageIndex { get; set; }

        public bool IsStructure { get; set; }

        public int ParentID { get; set; }

        public string Path { get; set; }

        #endregion
    }
}
