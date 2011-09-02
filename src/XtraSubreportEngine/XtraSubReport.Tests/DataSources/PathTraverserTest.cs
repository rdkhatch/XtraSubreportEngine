using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreportEngine;
using XtraSubreport.Engine;
using System.Collections;
using NorthwindOData.Northwind;
using DevExpress.XtraReports.UI;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class PathTraverserTest
    {
        [TestMethod]
        public void should_allow_root_collection()
        {
            object products = TestHelper.GetNorthwindProducts();

            object products2 = ObjectGraphPathTraverser.TraversePath(products, "");

            Assert.ReferenceEquals(products, products2);
        }

        [TestMethod]
        public void should_extract_item_from_collection()
        {
            object products = TestHelper.GetNorthwindProducts();

            object product = ObjectGraphPathTraverser.TraversePath(products, "[0]");

            Assert.IsTrue(product is NorthwindOData.Northwind.Product);
            Assert.IsNotNull(product);
        }

        [TestMethod]
        public void should_traverse_up_to_collections()
        {
            object products = TestHelper.GetNorthwindProducts();

            object order_details = ObjectGraphPathTraverser.TraversePath(products, "[0].Order_Details");

            var collection = (order_details as IEnumerable).Cast<Order_Detail>();

            Assert.IsNotNull(collection);
        }

        [TestMethod]
        public void should_traverse_through_collections_with_index()
        {
            object products = TestHelper.GetNorthwindProducts();

            object order_detail = ObjectGraphPathTraverser.TraversePath(products, "[0].Order_Details[0]");

            Assert.IsTrue(order_detail is Order_Detail);
        }

        [TestMethod]
        public void should_traverse_through_collections_without_index()
        {
            object products = TestHelper.GetNorthwindProducts();

            object order_details = ObjectGraphPathTraverser.TraversePath(products, "Order_Details");

            Assert.IsTrue(order_details is IEnumerable<Order_Detail>);
        }

        [TestMethod]
        public void combine_parent_datasourcepath_with_band_memberpath()
        {
            var form = TestHelper.CreateDesignForm();
            var factory = new ReportFactory();

            // Parent Report
            var report = factory.GetNewReport();
            report.Name = "parentreport";
            var datasource = TestHelper.NorthwindDataSource;
            datasource.DataSourceRelationPath = "[0]";
            report.SelectedDesignTimeDataSource = datasource;

            // Subreport Container
            var detailReportBand = new DetailReportBand();
            detailReportBand.DataMember = "OrderDetails";
            report.Bands.Add(detailReportBand);

            var path = detailReportBand.GetFullDataMemberPath();
            Assert.AreEqual("[0].OrderDetails", path);
        }
    }
}
