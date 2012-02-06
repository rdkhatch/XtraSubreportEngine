using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using XtraSubreport.Engine.RuntimeActions.Providers;

namespace XtraSubReport.Tests.RuntimeActions
{
    [TestFixture]
    public class RuntimeActionMefTest
    {
        [Test]
        public void should_find_exports()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var provider = new ReportMEFRuntimeActionProvider(basePath, false);

            // Assert ColorReplaceAction exists
            Assert.AreEqual(1, provider.GetRuntimeActions().Count());
        }
    }
}
