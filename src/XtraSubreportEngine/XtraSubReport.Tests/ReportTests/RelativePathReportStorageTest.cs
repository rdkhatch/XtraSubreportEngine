using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using NUnit.Framework;
using XtraSubreport.Engine.Support;

namespace XtraSubReport.Tests.Support
{
    [TestFixture]
    public class RelativePathReportStorageTest
    {
        string applicationBasePath;
        string reportsRelativeBasePath;
        string reportsFullBasePath;

        XtraReport report1;
        string report1RelativePath;
        string report1FullPath;

        RelativePathReportStorage storage;

        [SetUp]
        public void Init()
        {
            // C:\...\TestDirectory\Out\Reports
            var reportPath = "Reports";
            Init(reportPath);
        }

        private void Init(string _reportsRelativeBasePath)
        {
            //var outFolder = TestContext.DeploymentDirectory;        // Visual Studio Tests
            var outFolder = TestContext.CurrentContext.TestDirectory; // NUnit

            applicationBasePath = outFolder;

            reportsRelativeBasePath = _reportsRelativeBasePath;
            reportsFullBasePath = Path.Combine(applicationBasePath, reportsRelativeBasePath);

            // Clear Folder
            if (Directory.Exists(reportsFullBasePath))
                Directory.Delete(reportsFullBasePath, true);

            // Create Folder
            Directory.CreateDirectory(reportsFullBasePath);

            // Create Report
            report1 = new XtraReport() { Name = "Report1" };
            report1RelativePath = "Report1.resx";
            report1FullPath = Path.Combine(reportsFullBasePath, report1RelativePath);

            // Create Storage Extension
            storage = new RelativePathReportStorage(reportsRelativeBasePath);
        }

        [Test]
        public void Should_Load_From_Relative_Base_Path()
        {
            report1.Name = "TestLoad";

            // Save Report (using Full Path)
            report1.SaveLayout(report1FullPath);
            Assert.IsTrue(File.Exists(report1FullPath));

            // Open Report - using Report Storage
            var bytes = storage.GetData(report1RelativePath);
            var stream = new MemoryStream(bytes);
            var openedReport = XtraReport.FromStream(stream, true);

            // Assert Opened Properly
            Assert.AreEqual(report1.Name, openedReport.Name);
        }

        [Test]
        public void Should_Save_To_Relative_Base_Path()
        {
            report1.Name = "TestSave";

            // Save Report - using Report Storage
            storage.SetData(report1, report1RelativePath);

            // Assert File Exists
            Assert.IsTrue(File.Exists(report1FullPath));

            // Assert Saved Properly
            var savedReport = XtraReport.FromFile(report1FullPath, true);
            Assert.AreEqual(report1.Name, savedReport.Name);
        }

        [Test]
        public void Should_Save_To_Relative_Base_Path_from_save_dialog_Within_bin_path()
        {
            report1.Name = "TestSave";

            // Save Dialog uses this url format
            // ~\Reports\Report1.resx
            var saveDialogRelativeFilePath = Path.Combine("~", reportsRelativeBasePath, report1RelativePath);

            // Save Report - using Report Storage
            storage.SetData(report1, saveDialogRelativeFilePath);

            // Assert File Exists
            Assert.IsTrue(File.Exists(report1FullPath));

            // Assert Saved Properly
            var savedReport = XtraReport.FromFile(report1FullPath, true);
            Assert.AreEqual(report1.Name, savedReport.Name);
        }

        [Test]
        public void Should_Save_To_Relative_Base_Path_from_save_dialog_Outside_bin_path()
        {
            // C:\...\TestDirectory\Reports  (Notice this is outside Out [bin] path)
            var reportPath = @"..\Reports";

            // Re-Initialize using new report base path (creates directory, report, etc.)
            Init(reportPath);

            report1.Name = "TestSave";

            // Save Dialog uses this url format
            // ~\Reports\Report1.resx
            var saveDialogRelativeFilePath = Path.Combine("~", reportsRelativeBasePath, report1RelativePath);

            // Save Report - using Report Storage
            storage.SetData(report1, saveDialogRelativeFilePath);

            // Assert File Exists
            Assert.IsTrue(File.Exists(report1FullPath));

            // Assert Saved Properly
            var savedReport = XtraReport.FromFile(report1FullPath, true);
            Assert.AreEqual(report1.Name, savedReport.Name);
        }

        [Test]
        public void Should_Recognize_Is_Valid()
        {
            // Assert Invalid URL
            Assert.IsFalse(storage.IsValidUrl(report1RelativePath));

            // Save Report (using Full Path)
            report1.SaveLayout(report1FullPath);
            Assert.IsTrue(File.Exists(report1FullPath));

            // Assert Valid URL
            Assert.IsTrue(storage.IsValidUrl(report1RelativePath));
        }

        [Test]
        public void Test_Directory_Should_Be_Empty()
        {
            // Each Test should start with an empty directory
            var files = Directory.GetFiles(reportsRelativeBasePath).ToList();
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
