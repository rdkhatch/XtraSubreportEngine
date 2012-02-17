using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using NorthwindOData.Northwind;
using NUnit.Framework;
using XtraSubReport.Tests.Support;
using XtraSubReports.TestResources.Infrastructure;
using XtraSubreport.Design;
using XtraSubreport.Design.Traversals;
using XtraSubreport.Engine;
using XtraSubreport.Engine.Support;

namespace XtraSubReport.Tests
{
    [TestFixture]
    public class PathTraverserTest
    {
        [Test]
        public void should_allow_root_collection()
        {
            var orders = TestHelper.GetNorthwindOrders();

            // Null
            var orders2 = new ObjectGraphPathTraverser().Traverse(orders, null);

            orders.Should().BeSameAs(orders2,"Traversal result should be the same");
            
            // Empty String
            object orders3 = new ObjectGraphPathTraverser().Traverse(orders, "");
            Assert.AreSame(orders, orders3);
        }

        [Test]
        public void should_extract_item_from_collection()
        {
            object orders = TestHelper.GetNorthwindOrders();

            object order = new ObjectGraphPathTraverser().Traverse(orders, "[0]");

            Assert.IsTrue(order is NorthwindOData.Northwind.Order);
            Assert.IsNotNull(order);
        }

        [Test]
        public void should_traverse_up_to_collections()
        {
            object orders = TestHelper.GetNorthwindOrders();

            object order_details = new ObjectGraphPathTraverser().Traverse(orders, "[0].Order_Details");

            var collection = (order_details as IEnumerable).Cast<Order_Detail>();

            Assert.IsNotNull(collection);
        }

        [Test]
        public void should_traverse_through_collections_with_index()
        {
            object orders = TestHelper.GetNorthwindOrders();

            object order_detail = new ObjectGraphPathTraverser().Traverse(orders, "[0].Order_Details[0]");

            Assert.IsTrue(order_detail is Order_Detail);
        }

        [Test]
        public void should_traverse_through_collections_without_index()
        {
            object orders = TestHelper.GetNorthwindOrders();

            object order_details = new ObjectGraphPathTraverser().Traverse(orders, "Order_Details");

            Assert.IsTrue(order_details is IEnumerable<Order_Detail>);
        }

    }
}
