using System;
using System.Collections.Generic;
using System.Linq;
using XtraSubreportEngine.Support;
using XtraSubreportEngine;
using DevExpress.XtraReports.UI;
using GeniusCode.Framework.Extensions;
using XtraSubreport.Engine.Support;
using XtraSubreport.Engine.Extensions;
using System.ComponentModel.Composition.Hosting;
using XtraSubreport.Contracts;
using System.IO;
using GeniusCode.Framework.Support.Collections.Tree;
using XtraSubreport.Contracts.DataSources;

namespace XtraSubreport.Engine
{
    public static class DesignTimeHelper
    {
        public static bool PopulateDesignTimeDataSource(MyReportBase report)
        {
            var selectedSource = report.DesignTimeDataSources.FirstOrDefault();

            return PopulateDesignTimeDataSource(report, selectedSource);
        }

        public static bool PopulateDesignTimeDataSource(MyReportBase report, DesignTimeDataSourceDefinition definition)
        {
            // Set selected Design Time DataSource
            // Also sets .DataSource property
            report.SelectedDesignTimeDataSource = definition;

            return report.DataSource != null;
        }

        public static void PassDesignTimeDataSourceToSubreport(SubreportBase container, MyReportBase subreport)
        {
            var parentReport = (MyReportBase)container.RootReport;

            var parentDataSourceItem = parentReport.SelectedDesignTimeDataSource;

            var path = DesignTimeHelper.GetFullDataMemberPath(container.Band);

            var datasourceItem = new DesignTimeDataSourceDefinition(parentDataSourceItem.DataSourceName, parentDataSourceItem.DataSourceAssemblyLocationPath, path);

            // Go!
            subreport.SelectedDesignTimeDataSource = datasourceItem;
        }

        /// <summary>
        /// Concatenates all nested DataMember paths to create the Full DataMember Path
        /// </summary>
        /// <param name="band"></param>
        /// <returns></returns>
        public static string GetFullDataMemberPath(this Band band)
        {
            string path = string.Empty;

            if (band == null)
                return path;

            band.TryAs<XtraReportBase>(report =>
            {
                var context = report.GetReportDataContext();
                path = context.GetDataMemberDisplayName(report.DataSource, report.DataMember);

                bool isRootReport = report == report.Report;

                // Keep calling parent reports, until there is no DataMember
                if (!isRootReport && path != string.Empty)
                {
                    // Go deeper
                    string parentPath = GetFullDataMemberPath(report.Report);
                    path = "{0}.{1}".FormatString(parentPath, path);

                    // Prevent a period at the beginning
                    if (path.StartsWith("."))
                        path = path.Remove(0, 1);
                }
                else
                {
                    // If we are at the top, add starting Relation Path from datasource
                    report.TryAs<MyReportBase>(myReport =>
                        {
                            if (myReport.SelectedDesignTimeDataSource != null)
                            {
                                // Append parent report
                                var startingReportPath = myReport.SelectedDesignTimeDataSource.DataSourceRelationPath;
                                path = startingReportPath;
                            }
                        });
                }
            });

            if (band is XtraReportBase == false)
            {
                var report = band.Report;

                // Recurse into Report, which has the DataMember for the Collection
                // Prevent infinite loop, when a report is it's own parent
                if (band != report)
                    path = GetFullDataMemberPath(report);
            }

            band.TryAs<DetailBand>(detail =>
                {
                    // Detail band is a single item
                    // By Default, Use first element for design-time
                    path += "[0]";
                });

            return path;
        }

        public static IEnumerable<DesignTimeDataSourceTreeItem> BuildDesignTimeDataSourceTreeItems(MyReportBase report, out DynamicTree<DesignTimeDataSourceTreeItem> tree, out List<DesignTimeDataSourceTreeItem> flatList)
        {
            var treeItems = BuildDesignTimeDataSourceTreeItems(report);

            Func<string, string, DesignTimeDataSourceTreeItem> structureBuilder = (string1, string2) =>
                {
                    return new DesignTimeDataSourceTreeItem()
                    {
                        Name = string1,
                        Path = string2,
                        IsStructure = true
                    };
                };

            var treeView = new TreeviewStructureBuilder<DesignTimeDataSourceTreeItem>();
            treeView.Delimiter = @"\";
            treeView.CreateTree(treeItems, structureBuilder, out tree, out flatList, DuplicateTreeItemBehavior.ShowOnlyOneItem);

            return treeItems;
        }

        public static IEnumerable<DesignTimeDataSourceTreeItem> BuildDesignTimeDataSourceTreeItems(MyReportBase report)
        {
            // Report Requested Datasource Definitions
            var requestedDatasources = report.DesignTimeDataSources;

            // Folders
            var requestedFolders = requestedDatasources.Select( definition => DataSourceProvider.FormatRelativePath(definition.DataSourceAssemblyLocationPath) );
            var realFolders = DataSourceProvider.GetAllFoldersWithinBasePathContainingDLLs().Select(folder => DataSourceProvider.FormatRelativePath(folder));
            var allFolders = requestedFolders.Union(realFolders);

            Func<string, IEnumerable<IReportDatasourceMetadata>> GetExportsFromRelativeFolder = relativeFolder =>
                {
                    var exports = DataSourceProvider.GetDatasources(relativeFolder);
                    var metadatas = exports.Select( (lazy) => lazy.Metadata );
                    return metadatas;
                };

            Func<IReportDatasourceMetadata, DesignTimeDataSourceDefinition, bool> match = (metadata, requested) =>
                {
                    if (metadata == null || requested == null)
                        return false;
                    else
                        return metadata.Name == requested.DataSourceName;
                };

            Func<string, IReportDatasourceMetadata, DesignTimeDataSourceDefinition, DesignTimeDataSourceTreeItem> CreateDataSourceTreeItem = (relativeFolder, metadataNullable, definitionNullable) =>
                {
                    var definition = definitionNullable ?? new DesignTimeDataSourceDefinition(metadataNullable.Name, relativeFolder, string.Empty);

                    return new DesignTimeDataSourceTreeItem()
                                {
                                    Path = relativeFolder,
                                    Name = definition.DataSourceName,
                                    
                                    DesignTimeDataSourceDefinition = definition,
                                    MEFMetadata = metadataNullable,
                                    PreviouslyUsedWithThisReport = (definitionNullable != null).ToString(),
                                    RelationPath = definition.DataSourceRelationPath
                                };
                };

            var dataSourceTreeItems =   (from relativeFolder in allFolders
                                        let exports = GetExportsFromRelativeFolder(relativeFolder)
                                        let definitions = requestedDatasources.Where(definition => definition.DataSourceAssemblyLocationPath == relativeFolder)
                                        // Join exports & definitions on folder + datasource name
                                        from tuple in exports.FullOuterJoin(definitions, match)
                                        let export = tuple.Item1
                                        let definition = tuple.Item2
                                        select CreateDataSourceTreeItem(relativeFolder, export, definition)).ToList();

            //var folderTreeItems = from relativeFolder in allFolders
            //                      select new DesignTimeDataSourceTreeItem()
            //                      {
            //                          Path = relativeFolder,
            //                          Name = relativeFolder
            //                      };

            return dataSourceTreeItems;
        }

    }
}
