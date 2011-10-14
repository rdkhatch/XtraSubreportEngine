using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreport.Engine.RuntimeActions.Providers;
using System.IO;
using System.Reflection;

namespace XtraSubReport.Tests.RuntimeActions
{
    [TestClass]
    public class RuntimeActionMefTest
    {
        [TestMethod]
        public void should_find_exports()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var provider = new ReportMEFRuntimeActionProvider(basePath, false);

            // Assert ColorReplaceAction exists
            Assert.AreEqual(1, provider.GetRuntimeActions().Count());
        }
    }
}
