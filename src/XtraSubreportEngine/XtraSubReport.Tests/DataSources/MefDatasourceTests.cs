using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreportEngine;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class MefDatasourceTests
    {
        [TestMethod]
        public void catalog_should_not_be_empty()
        {
            var exports = DataSourceLocator.GetDatasources(string.Empty).ToList();

            Assert.IsTrue(exports.Count > 0);
        }

        [TestMethod]
        public void Northwind_Orders_should_be_exported()
        {
            var match = DataSourceLocator.GetDatasource(TestHelper.NorthwindDataSource);

            Assert.IsNotNull(match);
        }

        //[TestMethod]
        //public void catalog_should_support_multiple_directories()
        //{
        //    //DesignTimeHelper.BuildDesignTimeDataSourceTreeItems(new MyReportBase());
        //    //var container = DataSourceProvider.Singleton._container;
        //}
    }
}
