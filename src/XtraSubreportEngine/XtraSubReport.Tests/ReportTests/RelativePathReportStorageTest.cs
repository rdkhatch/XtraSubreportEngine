using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreport.Engine.Support;

namespace XtraSubReport.Tests.Support
{
    [TestClass]
    public class RelativePathReportStorageTest
    {
        string basePath;

        XtraReport report1;
        string report1Filename;
        string report1Fullpath;

        RelativePathReportStorage storage;

        [TestInitialize]
        public void Init()
        {
            var outFolder = TestContext.TestRunDirectory;

            // C:\Out\Reports
            basePath = Path.Combine(outFolder, "Reports");

            // Clear Folder
            if (Directory.Exists(basePath))
                Directory.Delete(basePath, true);

            // Create Folder
            Directory.CreateDirectory(basePath);

            // Create Report
            report1 = new XtraReport() { Name = "Report1" };
            report1Filename = "Report1.resx";
            report1Fullpath = Path.Combine(basePath, report1Filename);

            // Create Storage Extension
            storage = new RelativePathReportStorage(basePath);
        }

        [TestMethod]
        public void Should_Load_From_Relative_Base_Path()
        {
            report1.Name = "TestLoad";

            // Save Report (using Full Path)
            report1.SaveLayout(report1Fullpath);

            // Assert File Exists
            Assert.IsTrue(File.Exists(report1Fullpath));

            // Open Report - using Report Storage
            var bytes = storage.GetData(report1Filename);
            var stream = new MemoryStream(bytes);
            var openedReport = XtraReport.FromStream(stream, true);

            // Assert Opened Properly
            Assert.AreEqual(report1.Name, openedReport.Name);
        }

        [TestMethod]
        public void Should_Save_To_Relative_Base_Path()
        {
            report1.Name = "TestSave";

            // Save Report - using Report Storage
            storage.SetData(report1, report1Filename);

            // Assert File Exists
            Assert.IsTrue(File.Exists(report1Fullpath));

            // Assert Saved Properly
            var savedReport = XtraReport.FromFile(report1Fullpath, true);
            Assert.AreEqual(report1.Name, savedReport.Name);
        }

        [TestMethod]
        public void Should_Recognize_Is_Valid()
        {
            // Assert Invalid URL
            Assert.IsFalse(storage.IsValidUrl(report1Filename));

            // Save Report (using Full Path)
            report1.SaveLayout(report1Fullpath);

            // Assert File Exists
            Assert.IsTrue(File.Exists(report1Fullpath));

            // Assert Valid URL
            Assert.IsTrue(storage.IsValidUrl(report1Filename));
        }

        //[TestMethod]
        //public void test_dialog()
        //{
        //    var url = storage.GetNewUrl();
        //}

        [TestMethod]
        public void Test_Directory_Should_Be_Empty()
        {
            // Each Test should start with an empty directory
            var files = Directory.GetFiles(basePath).ToList();
            Assert.AreEqual(0, files.Count);
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

    }
}
