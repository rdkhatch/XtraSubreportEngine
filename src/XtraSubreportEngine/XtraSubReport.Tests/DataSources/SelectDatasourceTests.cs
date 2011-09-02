using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreport.Engine;
using XtraSubreportEngine.Support;
using XtraSubreportEngine;
using System.Reflection;
using System.IO;
using XtraSubreport.Engine.Support;
using GeniusCode.Framework.Support.Collections.Tree;
using GeniusCode.Framework.Extensions;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class SelectDatasourceTests
    {
        #region Test initialize
        static string binFolder;
        static List<string> FoldersCreated = new List<string>();


        [TestInitialize]
        public void Init()
        {
            binFolder = DataSourceProvider.GetBasePath();

            FoldersCreated.Add(@"TestFolder");
            FoldersCreated.Add(@"TestFolder\TestChildFolder");

            CreateFolders();
            CopyDLLsIntoFolders();
        }

        private void CreateFolders()
        {
            FoldersCreated.ForEach(relativeFolder =>
                {
                    var fullFolder = Path.Combine(binFolder, relativeFolder);
                    Directory.CreateDirectory(fullFolder);
                });
        }

        private void CopyDLLsIntoFolders()
        {
            var dlls = Directory.EnumerateFiles(binFolder, "*.dll");
            var childRelativeFolder = FoldersCreated[1];
            var destinationFolder = Path.Combine(binFolder, childRelativeFolder);
            dlls.ToList().ForEach(dll =>
                {
                    File.Copy(dll, Path.Combine(destinationFolder, Path.GetFileName(dll)), true);
                });
        }

        private void DeleteFolders()
        {
            FoldersCreated.ForEach(relativeFolder =>
                {
                    var fullFolder = Path.Combine(binFolder, relativeFolder);
                    var isChildFolder = Path.GetFullPath(binFolder) != Path.GetFullPath(fullFolder);

                    // Delete Folder
                    if (isChildFolder && Directory.Exists(fullFolder))
                    {
                        Directory.EnumerateFiles(fullFolder).ForAll(file => File.Delete(file));
                        Directory.Delete(fullFolder, true);
                    }
                });
        }

        [TestCleanup]
        public void Cleanup()
        {
            DeleteFolders();
        }
        #endregion

        [TestMethod]
        public void Should_Return_Folders()
        {
            var folders = DataSourceProvider.GetAllFoldersWithinBasePathContainingDLLs();

            Assert.IsTrue(folders.Count() == 2);
        }

        [TestMethod]
        public void Should_Create_TreeItems_With_Exports_For_Folders()
        {
            var report = new MyReportBase();
            var northwind = TestHelper.NorthwindDataSource;
            var fakeDefinition1 = new DesignTimeDataSourceDefinition(northwind.DataSourceName, @"TestFolder", string.Empty);
            var fakeDefinition2 = new DesignTimeDataSourceDefinition(northwind.DataSourceName, @"FakeFolder", string.Empty);
            var fakeDefinition3 = new DesignTimeDataSourceDefinition(northwind.DataSourceName, @"TestFolder\TestChildFolder", string.Empty);
            report.DesignTimeDataSources.Add(northwind);
            report.DesignTimeDataSources.Add(fakeDefinition1);
            report.DesignTimeDataSources.Add(fakeDefinition2);
            report.DesignTimeDataSources.Add(fakeDefinition3);

            DynamicTree<DesignTimeDataSourceTreeItem> tree;
            List<DesignTimeDataSourceTreeItem> flatList;

            var treeItems = DesignTimeHelper.BuildDesignTimeDataSourceTreeItems(report, out tree, out flatList).ToList();

            //  Show TreeView, debug only
            //var form = new SelectDesignTimeDataSourceForm(report);
            //form.ShowDialog();

            Assert.AreEqual(5, treeItems.Count);
        }
    }
}
