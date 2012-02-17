using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Nodes.Operations;
using GeniusCode.Framework.Extensions;
using GeniusCode.Framework.Support.Collections.Tree;
using XtraSubReport.Winforms.Popups.Support;
using XtraSubreport.Contracts.DesignTime;
using XtraSubreport.Design;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubReport.Winforms.Popups
{
    public enum ImageIndex : int
    {
        Folder = 0,
        DataSource,
        Unavailable
    }

    public partial class SelectDesignTimeDataSourceForm : Form
    {
        private readonly IDesignDataContext _context;

        readonly MyReportBase _report;
        readonly Action<ReportDatasourceMetadataWithTraversal> _callback;

        public SelectDesignTimeDataSourceForm(IDesignDataContext context, MyReportBase report, Action<ReportDatasourceMetadataWithTraversal> callback)
        {
            InitializeComponent();

            _context = context;
            _report = report;
            _callback = callback;
        }

        private void SelectDesignTimeDataSourceForm_Load(object sender, EventArgs e)
        {
            DynamicTree<DesignTimeDataSourceTreeItem> tree;
            List<DesignTimeDataSourceTreeItem> flatList;

            BuildDesignTimeDataSourceTreeItems(_report, out tree, out flatList);

            // Set Images
            flatList.ForEach(SetImageIndex);

            // Populate TreeList
            bindingSource1.DataSource = flatList;

            // Expand nodes
            treeList1.ExpandAll();

            // Highlight current datasource
            HighlightCurrentDatasource();
        }

        private IEnumerable<DesignTimeDataSourceTreeItem> BuildDesignTimeDataSourceTreeItems(MyReportBase report, out DynamicTree<DesignTimeDataSourceTreeItem> tree, out List<DesignTimeDataSourceTreeItem> flatList)
        {
            var treeItems = BuildDesignTimeDataSourceTreeItems(report).ToList();

            Func<string, string, DesignTimeDataSourceTreeItem> structureBuilder = (string1, string2) =>
            {
                return new DesignTimeDataSourceTreeItem()
                {
                    Name = string1,
                    Path = string2,
                    IsStructure = true
                };
            };

            var treeView = new TreeviewStructureBuilder<DesignTimeDataSourceTreeItem> {Delimiter = @"\"};
            treeView.CreateTree(treeItems, structureBuilder, out tree, out flatList, DuplicateTreeItemBehavior.ShowOnlyOneItem);

            return treeItems;
        }

        private IEnumerable<DesignTimeDataSourceTreeItem> BuildDesignTimeDataSourceTreeItems(MyReportBase report)
        {
            // Report Requested Datasource Definitions
            var requestedDatasources = _context.DesignDataDefinitionRepository.GetDesignTimeDatasourcesForReport(report);

            Func<IReportDatasourceMetadata, ReportDatasourceMetadataWithTraversal, bool> match = (metadata, requested) =>
            {
                if (metadata == null || requested == null)
                    return false;
                else
                    return metadata.UniqueId == requested.DataSourceName;
            };

            Func<IReportDatasourceMetadata, ReportDatasourceMetadataWithTraversal, DesignTimeDataSourceTreeItem> createDataSourceTreeItem = (metadataNullable, definitionNullable) =>
            {
                var definition = definitionNullable ?? ReportDatasourceMetadataWithTraversal.CreateRootDefinitionFromMetadata(metadataNullable);

                return new DesignTimeDataSourceTreeItem()
                {
                    Path = string.Empty,
                    Name = definition.DataSourceName,

                    DesignTimeDataSourceDefinition = definition,
                    Metadata = metadataNullable,
                    PreviouslyUsedWithThisReport = (definitionNullable != null).ToString(),
                    RelationPath = definition.DataSourceRelationPath
                };
            };

            var dataSourceTreeItems = (from datasourceProvider in _context.Providers
                                       let availableDatasources = datasourceProvider.GetReportDatasources()
                                       // Join availableDatasources & requestedDatasources on datasource name
                                       from tuple in availableDatasources.FullOuterJoin(requestedDatasources, match)
                                       let export = tuple.T1Object
                                       let definition = tuple.T2Object
                                       select createDataSourceTreeItem(export, definition)).ToList();

            return dataSourceTreeItems;
        }

        private static void SetImageIndex(DesignTimeDataSourceTreeItem item)
        {
            ImageIndex index;

            if (item.IsStructure)
                index = ImageIndex.Folder;
            else if (item.IsDatasourceAvailable)
                index = ImageIndex.DataSource;
            else
                index = ImageIndex.Unavailable;

            item.ImageIndex = (int)index;
        }

        private IEnumerable<TreeListNode> GetAllNodes(TreeList tree)
        {
            var accumulator = new TreeListOperationAccumulateNodes();
            tree.NodesIterator.DoOperation(accumulator);

            return accumulator.Nodes.Cast<TreeListNode>();
        }

        void HighlightCurrentDatasource()
        {
            treeList1.Selection.Clear();

            var selectedDatasource = _context.DesignDataDefinitionRepository.GetCurrentAssociationForReport(_report);//;. .GetSelectedDesignTimeDatasourceForReport(_report);
            

            if (selectedDatasource == null)
                return;

            // Get Index to highlight
            var datasource = GetDatasource();
            var selectedTreeItem = datasource.Single(treeItem => treeItem.DesignTimeDataSourceDefinition == selectedDatasource);
            var index = datasource.IndexOf(selectedTreeItem);

            // Get Node at Index
            var selectedTreeNode = GetAllNodes(treeList1).ElementAt(index);
            selectedTreeNode.Selected = true;
        }

        void cmdSelectDataSource_Click(object sender, EventArgs e)
        {
            var datasource = GetDatasource();

            var selection = from node in treeList1.Selection.OfType<TreeListNode>()
                            let index = node.Id
                            let treeItem = datasource[index]
                            where treeItem.IsDatasourceAvailable
                            where treeItem.IsStructure == false
                            select treeItem;

            var selectionList = selection.ToList();

            if (selectionList.Count() != 1)
            {
                MessageBox.Show("Please select one Datasouce");
                return;
            }

            var definition = selectionList.First().DesignTimeDataSourceDefinition;
            _callback.Invoke(definition);
        }

        List<DesignTimeDataSourceTreeItem> GetDatasource()
        {
            return (bindingSource1.DataSource as IEnumerable<DesignTimeDataSourceTreeItem>).ToList();
        }

    }
}
