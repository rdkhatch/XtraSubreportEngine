using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtraSubreport.Contracts;
using System.ComponentModel.Composition;
using NorthwindOData.Northwind;
using XtraSubreport.Contracts.DataSources;

namespace NorthwindOData
{
    [ReportDatasourceExport("Northwind_Products", "Products and Order_Details from Northwind OData Feed")]
    public class NorthwindDataSource : IReportDatasource
    {
        private IEnumerable<Product> datasource;
        public IEnumerable<Product> GetDataSource()
        {
            if (datasource == null)
                //MockNorthwindObjectGraph();
                FetchData();
            return datasource;
        }

        // Only use this method if OData feed goes down
        private void MockNorthwindObjectGraph()
        {
            var product = new Product();
            var order_detail = new Order_Detail();

            product.Order_Details.Add(order_detail);

            var graph = new List<Product>();
            graph.Add(product);

            datasource = graph;
        }

        private void FetchData()
        {
            // Public Northwind OData Service
            var t = new NorthwindEntities(new Uri("http://services.odata.org/Northwind/Northwind.svc/"));
            var q = from t2 in t.Products.Expand("Order_Details") select t2;
            datasource = q.ToList();
        }


        public object DataSource
        {
            get { return GetDataSource(); }
        }
    }
}
