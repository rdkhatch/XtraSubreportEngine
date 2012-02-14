using System;
using GeniusCode.Framework.Support.Objects;

namespace XtraSubreportEngine.Support
{
    public class DesignTimeDataSourceDefinition : XRSerializableBase
    {
        // Default constructor, for IXRSerializable deserializing
        public DesignTimeDataSourceDefinition()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public DesignTimeDataSourceDefinition(string datasourceUniqueId, string dataSourceName, string dataSourceRelationPath)
        {
            DataSourceUniqueId = datasourceUniqueId;
            DataSourceName = dataSourceName;

            // Optional
            DataSourceRelationPath = dataSourceRelationPath;
        }

        public string DataSourceUniqueId { get; set; }
        public string DataSourceName { get; set; }
        public string DataSourceRelationPath { get; set; }

        [Obsolete]
        public string DataSourceAssemblyLocationPath { get; set; }

        // These get assigned in the Designer when the datasource is opened
        public Type RootDataSourceType { get; set; }
        public Type DataSourceType { get; set; }

        public override bool Equals(object obj)
        {
            return CompareHelper.ObjectCompare(this, obj, CompareMethod.GetHashCodeIfSameTypeOtherwiseFalse);
        }

        public override int GetHashCode()
        {
            return HashcodeBuilder.GetHashcodeItems(DataSourceUniqueId, DataSourceRelationPath);
        }

        public override string ToString()
        {
            return "{0} ('{1}')".FormatString(DataSourceName, DataSourceRelationPath);
        }

    }

}
