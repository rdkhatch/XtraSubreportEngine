using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NorthwindOData.Northwind;

namespace NorthwindOData
{
    //[ReportDatasourceExport("Northwind_Products", "WE CHANGED THIS Products from Northwind OData Feed")]
    public class NorthwindProducts
    {
        private IEnumerable<Product> datasource;
        public string SerializeToFilename = "Northwind_Products.json";

        public IEnumerable<Product> GetDataSource()
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
            datasource = new List<Product>()
            {
                new Product() { ProductName = "Product One" },
                new Product() { ProductName = "Product Two" }
            };
        }

        private string GetSerializedFilePath()
        {
            return this.SerializeToFilename;
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

                datasource = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
            }
        }

        private void FetchData()
        {
            // Public Northwind OData Service
            var t = new NorthwindEntities(new Uri("http://services.odata.org/Northwind/Northwind.svc/"));
            var q = (from product in t.Products
                     select product).Take(25);
            datasource = q.ToList();
        }

    }
}
