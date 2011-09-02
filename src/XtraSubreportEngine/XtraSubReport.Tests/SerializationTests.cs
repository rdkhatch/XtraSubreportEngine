using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreportEngine.Support;
using System.IO;
using XtraSubreport.Engine;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void can_serialize_custom_report_properties()
        {
            var report1 = new DummyReport();

            var abc = new DesignTimeDataSourceDefinition("a", "b", "c");
            report1.RyansSerializeTestObject = abc;

            var report2 = TestHelper.RunThroughSerializer(report1);

            Assert.IsNotNull(report2.RyansSerializeTestObject);
            Assert.AreEqual(report1.RyansSerializeTestObject, report2.RyansSerializeTestObject);
        }

        [TestMethod]
        public void datasources_list_is_serialized()
        {
            var form = TestHelper.CreateDesignForm();
            var factory = new ReportFactory();

            // Stream
            var stream = new MemoryStream();

            var abc = new DesignTimeDataSourceDefinition("a", "b", "c");
            var xyz = new DesignTimeDataSourceDefinition("x", "y", "z");

            var report1 = (MyReportBase)factory.GetDefaultReport();

            // Collection
            report1.DesignTimeDataSources.Add(abc);
            report1.DesignTimeDataSources.Add(xyz);

            var report2 = TestHelper.RunThroughSerializer(report1);
            Assert.IsTrue(report2.DesignTimeDataSources.Count == 2);
        }
    }
}
