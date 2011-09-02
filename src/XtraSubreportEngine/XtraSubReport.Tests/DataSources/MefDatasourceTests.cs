using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreportEngine;
using XtraSubreport.Engine;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class MefDatasourceTests
    {
        [TestMethod]
        public void catalog_should_not_be_empty()
        {
            var exports = DataSourceProvider.GetDatasources(string.Empty).ToList();

            Assert.IsTrue(exports.Count > 0);
        }
        
        [TestMethod]
        public void Northwind_Products_should_be_exported()
        {
            var match = DataSourceProvider.GetDatasource(TestHelper.NorthwindDataSource);

            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void catalog_should_support_multiple_directories()
        {
            //DesignTimeHelper.BuildDesignTimeDataSourceTreeItems(new MyReportBase());
            //var container = DataSourceProvider.Singleton._container;
        }
    }
}
