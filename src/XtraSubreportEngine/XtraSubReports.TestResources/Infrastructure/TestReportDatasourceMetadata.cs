using System;
using System.Linq;
using XtraSubReports.TestResources.Models;
using XtraSubreport.Contracts.DesignTime;

namespace XtraSubReports.TestResources.Infrastructure
{
    public class TestReportDatasourceMetadata : IReportDatasourceMetadata
    {
        public TestReportDatasourceMetadata(string uniqueId)
        {
            UniqueId = uniqueId;
        }

        public string UniqueId { get; private set; }

        public string Name
        {
            get { return "Person2"; }
        }

        public string Description
        {
            get { return "This is some people"; }
        }

        public Type DataSourceType
        {
            get { return typeof(Person2); }
        }
    }
}