using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NorthwindOData.Northwind;
using XtraSubreport.Contracts.DataSources;

namespace NorthwindOData
{
    [ReportDatasourceExport("Northwind_Orders", "Orders, Order_Details, and Products from Northwind OData Feed")]
    public class NorthwindOrders : IReportDatasource
    {
        private IEnumerable<Order> datasource;
        public static readonly string SerializeToFilename = "Northwind_Orders.json";

        public IEnumerable<Order> GetDataSource()
        {
            if (datasource == null)
            {
                //MockNorthwindObjectGraph();
                DeserializeDataLocally();

                if (datasource == null)
                {
                    FetchData();
                    SerializeDataLocally();
                }

            }
            return datasource;
        }

        // Only use this method if OData feed goes down & we have nothing serialized (which yes - has happened before)
        private void MockNorthwindObjectGraph()
        {
            var order = new Order();

            var order_detail1 = new Order_Detail();
            var order_detail2 = new Order_Detail();

            order.Order_Details.Add(order_detail1);
            order.Order_Details.Add(order_detail2);

            var graph = new List<Order>();
            graph.Add(order);

            datasource = graph;
        }

        private string GetSerializedFilePath()
        {
            var directory = Path.GetDirectoryName(typeof(NorthwindOrders).Assembly.Location);
            var fullPath = Path.Combine(directory, NorthwindOrders.SerializeToFilename);
            return fullPath;
        }

        private void SerializeDataLocally()
        {
            var data = GetDataSource();

            var settings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            File.WriteAllText(GetSerializedFilePath(), json);
        }

        private void DeserializeDataLocally()
        {
            datasource = null;

            bool fileExists = File.Exists(GetSerializedFilePath());
            if (fileExists)
            {
                var json = File.ReadAllText(GetSerializedFilePath());

                datasource = JsonConvert.DeserializeObject<IEnumerable<Order>>(json);
            }
        }

        private void FetchData()
        {
            // Public Northwind OData Service
            var t = new NorthwindEntities(new Uri("http://services.odata.org/Northwind/Northwind.svc/"));
            var q = (from order in t.Orders.Expand("Order_Details/Product")
                     select order).Take(25);
            datasource = q.ToList();
        }

        object IReportDatasource.GetDataSource()
        {
            return GetDataSource();
        }
    }
}
