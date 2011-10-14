using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using GeniusCode.Framework.Extensions;
using GeniusCode.Framework.Support.Collections.Tree;
using XtraSubreport.Contracts.DataSources;
using XtraSubreport.Engine.Designer;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine
{
    public static class DesignTimeHelper
    {
        public static DesignTimeDataSourceDefinition GetSelectedDesignTimeDatasource(this MyReportBase report)
        {
            return report.DesignTimeDataSources.FirstOrDefault();
        }

        public static bool ChangeDesignTimeDatasourceToDefault(this MyReportBase report, IDesignerContext designContext)
        {
            var selectedSource = report.DesignTimeDataSources.FirstOrDefault();

            return ChangeDesignTimeDatasource(report, selectedSource, designContext);
        }

        public static bool ChangeDesignTimeDatasource(this MyReportBase report, DesignTimeDataSourceDefinition definition, IDesignerContext designContext)
        {
            object datasource = null;

            if (definition != null)
            {
                // Get Traversed Datasource
                var traversedResult = designContext.DataSourceLocator.GetTraversedObjectFromDataSourceDefinition(definition);
                datasource = traversedResult.TraversedDataSource;

                if (traversedResult.RootDataSource == null)
                    MessageBox.Show("Datasource: {0} could not be found or did not return a datasource.".FormatString(definition.ToString()));
                else
                {
                    // Store Definition on Report

                    // If already in list, Remore & Re-add, so as to move definition to index 0
                    if (report.DesignTimeDataSources.Contains(definition))
                    {
                        var index = report.DesignTimeDataSources.IndexOf(definition);
                        report.DesignTimeDataSources.RemoveAt(index);
                    }

                    // Add item as first in list - So we know it was the last one used
                    report.DesignTimeDataSources.Insert(0, definition);

                    // Verify Traversal worked
                    if (traversedResult.TraversedDataSource == null)
                        MessageBox.Show("Datasource: {0} was found, but the traversed Relation Path did not return a value.".FormatString(definition.ToString()));
                }
            }

            // Set DataSource
            report.SetReportDataSource(datasource);

            // Refresh Design Panel Fields List
            var designPanel = designContext.DesignForm.DesignMdiController.ActiveDesignPanel;
            if (designPanel != null)
                designPanel.Activate();

            return report.DataSource != null;
        }

        public static void SetupDesignTimeSubreportDatasourcePassing(this IDesignerContext designContext)
        {
            // Selected Subreport on Design Panel
            SubreportBase selectedSubreport = null;

            var controller = designContext.DesignForm.DesignMdiController;

            // Design-Time Event Handler
            controller.OnDesignPanelActivated(designPanel =>
            {
                var report = designPanel.Report;

                // TODO: Set DesignPanel Filename?
                // string url;
                // report.Extensions.TryGetValue("StorageID", out url);
                // designPanel.FileName = url;

                // Populate Design-Time Datasource
                report.TryAs<MyReportBase>(myReport =>
                {
                    if (selectedSubreport == null)
                        // Stand-alone Report
                        myReport.ChangeDesignTimeDatasourceToDefault(designContext);
                    else
                        // Subreport was Double-clicked, new DesignPanel has been activated for it
                        // Pass Design-Time DataSource from Parent to Subreport
                        DesignTimeHelper.PassDesignTimeDataSourceToSubreport(selectedSubreport, myReport, designContext);
                });

                // Capture selected Subreport
                // So we can enable double-clicking Subreport
                designPanel.SelectionChanged += (sender, e) =>
                {
                    var selected = designPanel.GetSelectedComponents();
                    selectedSubreport = selected.OfType<SubreportBase>().SingleOrDefault();

#if DEBUG
                    if (selectedSubreport != null)
                    {
                        var path = selectedSubreport.Band.GetFullDataMemberPath();
                        Debug.WriteLine("You selected subreport with Path: {0}".FormatString(path));
                    }
#endif
                };

            });
        }

        public static void PassDesignTimeDataSourceToSubreport(SubreportBase container, MyReportBase subreport, IDesignerContext designContext)
        {
            var parentReport = (MyReportBase)container.RootReport;

            var parentDataSourceItem = parentReport.GetSelectedDesignTimeDatasource();

            if (parentDataSourceItem != null)
            {
                var path = DesignTimeHelper.GetFullDataMemberPath(container.Band);

                var datasourceDefinition = new DesignTimeDataSourceDefinition(parentDataSourceItem.DataSourceName, parentDataSourceItem.DataSourceAssemblyLocationPath, path);

                // Go!
                subreport.ChangeDesignTimeDatasource(datasourceDefinition, designContext);
            }
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
                            var selectedDatasourceDefinition = myReport.GetSelectedDesignTimeDatasource();

                            if (selectedDatasourceDefinition != null)
                            {
                                // Append parent report
                                var startingReportPath = selectedDatasourceDefinition.DataSourceRelationPath;
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

        #region Tree Items

        public static IEnumerable<DesignTimeDataSourceTreeItem> BuildDesignTimeDataSourceTreeItems(DataSourceLocator locator, MyReportBase report, out DynamicTree<DesignTimeDataSourceTreeItem> tree, out List<DesignTimeDataSourceTreeItem> flatList)
        {
            var treeItems = BuildDesignTimeDataSourceTreeItems(locator, report);

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

        private static IEnumerable<DesignTimeDataSourceTreeItem> BuildDesignTimeDataSourceTreeItems(DataSourceLocator locator, MyReportBase report)
        {
            // Report Requested Datasource Definitions
            var requestedDatasources = report.DesignTimeDataSources;

            // Folders
            var requestedFolders = requestedDatasources.Select(definition => locator.FormatRelativePath(definition.DataSourceAssemblyLocationPath));
            var realFolders = locator.GetAllFoldersWithinBasePathContainingDLLs().Select(folder => locator.FormatRelativePath(folder));
            var allFolders = requestedFolders.Union(realFolders);

            Func<string, IEnumerable<IReportDatasourceMetadata>> GetExportsFromRelativeFolder = relativeFolder =>
            {
                var exports = locator.GetDatasources(relativeFolder);
                var metadatas = exports.Select((lazy) => lazy.Metadata);
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

            var dataSourceTreeItems = (from relativeFolder in allFolders
                                       let exports = GetExportsFromRelativeFolder(relativeFolder)
                                       let definitions = requestedDatasources.Where(definition => definition.DataSourceAssemblyLocationPath == relativeFolder)
                                       // Join exports & definitions on folder + datasource name
                                       from tuple in exports.FullOuterJoin(definitions, match)
                                       let export = tuple.T1Object
                                       let definition = tuple.T2Object
                                       select CreateDataSourceTreeItem(relativeFolder, export, definition)).ToList();

            return dataSourceTreeItems;
        }

        #endregion

    }
}
