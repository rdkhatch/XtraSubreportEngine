using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using NorthwindOData.Northwind;
using NUnit.Framework;
using XtraSubreport.Designer;
using XtraSubreport.Engine;

namespace XtraSubReport.Tests
{
    [TestFixture]
    public class PathTraverserTest
    {
        [Test]
        public void should_allow_root_collection()
        {
            object orders = TestHelper.GetNorthwindOrders();

            // Null
            object orders2 = ObjectGraphPathTraverser.TraversePath(orders, null);
            Assert.ReferenceEquals(orders, orders2);

            // Empty String
            object orders3 = ObjectGraphPathTraverser.TraversePath(orders, "");
            Assert.ReferenceEquals(orders, orders3);
        }

        [Test]
        public void should_extract_item_from_collection()
        {
            object orders = TestHelper.GetNorthwindOrders();

            object order = ObjectGraphPathTraverser.TraversePath(orders, "[0]");

            Assert.IsTrue(order is NorthwindOData.Northwind.Order);
            Assert.IsNotNull(order);
        }

        [Test]
        public void should_traverse_up_to_collections()
        {
            object orders = TestHelper.GetNorthwindOrders();

            object order_details = ObjectGraphPathTraverser.TraversePath(orders, "[0].Order_Details");

            var collection = (order_details as IEnumerable).Cast<Order_Detail>();

            Assert.IsNotNull(collection);
        }

        [Test]
        public void should_traverse_through_collections_with_index()
        {
            object orders = TestHelper.GetNorthwindOrders();

            object order_detail = ObjectGraphPathTraverser.TraversePath(orders, "[0].Order_Details[0]");

            Assert.IsTrue(order_detail is Order_Detail);
        }

        [Test]
        public void should_traverse_through_collections_without_index()
        {
            object orders = TestHelper.GetNorthwindOrders();

            object order_details = ObjectGraphPathTraverser.TraversePath(orders, "Order_Details");

            Assert.IsTrue(order_details is IEnumerable<Order_Detail>);
        }

        [Test]
        public void combine_parent_datasourcepath_with_band_memberpath()
        {
            var factory = new ReportFactory();
            var designContext = TestHelper.CreateDesignerContext();

            // Parent Report
            var report = factory.GetNewReport();
            report.Name = "parentreport";
            var definition = TestHelper.NorthwindDataSource;
            definition.DataSourceRelationPath = "[0]";
            report.ChangeDesignTimeDatasource(definition, designContext);

            // Subreport Container
            var detailReportBand = new DetailReportBand();
            detailReportBand.DataMember = "OrderDetails";
            report.Bands.Add(detailReportBand);

            var path = detailReportBand.GetFullDataMemberPath();
            Assert.AreEqual("[0].OrderDetails", path);
        }

        [Test]
        public void should_traverse_when_changing_datasource()
        {
            var factory = new ReportFactory();
            var designContext = TestHelper.CreateDesignerContext();

            var report = factory.GetNewReport();

            var definition = TestHelper.NorthwindDataSource;
            definition.DataSourceRelationPath = "Order_Details";

            report.ChangeDesignTimeDatasource(definition, designContext);

            var datasource = report.DataSource;

            Assert.IsTrue(datasource is IEnumerable<Order_Detail>);
        }
    }
}
