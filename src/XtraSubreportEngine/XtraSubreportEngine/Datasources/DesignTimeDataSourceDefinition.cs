using System;
using System.Linq;
using DevExpress.XtraReports.Serialization;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using GeniusCode.Framework.Extensions;
using GeniusCode.Framework.Support.Refection;
using GeniusCode.Framework.Support.Objects;

namespace XtraSubreportEngine.Support
{
    public class DesignTimeDataSourceDefinition : XRSerializableBase
    {
        // Default constructor, for IXRSerializable deserializing
        public DesignTimeDataSourceDefinition()
            : this (string.Empty, string.Empty, string.Empty)
        {
        }

        public DesignTimeDataSourceDefinition(string dataSourceName, string dataSourceAssemblyLocationPath, string dataSourceRelationPath)
            : this (dataSourceName, dataSourceAssemblyLocationPath, dataSourceRelationPath, null)
        {
        }

        public DesignTimeDataSourceDefinition(string dataSourceName, string dataSourceAssemblyLocationPath, string dataSourceRelationPath, Type dataSourceType)
        {
            DataSourceName = dataSourceName;
            DataSourceAssemblyLocationPath = dataSourceAssemblyLocationPath;
            // Optional
            DataSourceRelationPath = dataSourceRelationPath;
            DataSourceType = dataSourceType ?? typeof(Object);
        }


        public string DataSourceName { get; set; }
        public string DataSourceAssemblyLocationPath { get; set; }
        public string DataSourceRelationPath { get; set; }
        public Type DataSourceType { get; set; }


        public override bool Equals(object obj)
        {
            return CompareHelper.ObjectCompare(this, obj, CompareMethod.GetHashCodeIfSameTypeOtherwiseFalse);
        }

        public override int GetHashCode()
        {
            return HashcodeBuilder.GetHashcodeItems(DataSourceName, DataSourceAssemblyLocationPath, DataSourceRelationPath);
        }

    }

}
