using System.Collections;
using System.Linq;
using NUnit.Framework;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Tests
{
    public static partial class AssertHelper
    {
        public static class Reports
        {
            public static void AssertDatasourceHasItems(MyReportBase report)
            {
                var datasource = report.DataSource;
                Assert.IsNotNull(datasource);

                var collection = (datasource as IEnumerable).Cast<object>().ToList();
                Assert.AreNotEqual(0, collection.Count);
            }
        }
    }
}
