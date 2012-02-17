using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtraSubReports.TestResources.Models;
using XtraSubreport.Contracts.DesignTime;

namespace XtraSubReports.TestResources.Infrastructure
{
    public class DogTimeReportDatasourceProvider : IReportDatasourceProvider
    {
        public IEnumerable<IReportDatasourceMetadata> GetReportDatasources()
        {
            return new[] { new TestReportDatasourceMetadata("DogTime") };
        }

        public object GetReportDatasource(string uniqueId)
        {
            return Person2.SampleData();
        }
    }
}
