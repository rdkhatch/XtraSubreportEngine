using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using GeniusCode.Framework.Support.Collections.Tree;
using XtraSubreport.Engine.Support;
using XtraSubreportEngine.Support;

namespace XtraSubreport.Engine
{
    public enum ImageIndex : int
    {
        Folder = 0,
        DataSource,
        Unavailable
    }

    public partial class SelectDesignTimeDataSourceForm : Form
    {

        MyReportBase _report;
        Action<DesignTimeDataSourceDefinition> _callback;

        public SelectDesignTimeDataSourceForm(MyReportBase report, Action<DesignTimeDataSourceDefinition> callback)
        {
            InitializeComponent();

            _report = report;
            _callback = callback;
        }

        private void SelectDesignTimeDataSourceForm_Load(object sender, EventArgs e)
        {
            DynamicTree<DesignTimeDataSourceTreeItem> tree;
            List<DesignTimeDataSourceTreeItem> flatList;

            var treeItems = DesignTimeHelper.BuildDesignTimeDataSourceTreeItems(_report, out tree, out flatList).ToList();

            // Set Images
            flatList.ForEach(item => SetImageIndex(item));

            // Populate TreeList
            bindingSource1.DataSource = flatList;

            // Expand nodes
            treeList1.ExpandAll();

            // Highlight current datasource
            HighlightCurrentDatasource();
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

        void HighlightCurrentDatasource()
        {
            treeList1.Selection.Clear();

            DesignTimeDataSourceDefinition lastDataSource = _report.SelectedDesignTimeDataSource ?? _report.DesignTimeDataSources.FirstOrDefault();

            if (lastDataSource == null)
                return;

            // Get Index to highlight
            var datasource = GetDatasource();
            var selectedTreeItem = datasource.Where(treeItem => treeItem.DesignTimeDataSourceDefinition == lastDataSource).Single();
            var index = datasource.IndexOf(selectedTreeItem);

            // Get Node at Index
            var selectedTreeNode = treeList1.GetAllNodes().ElementAt(index);
            selectedTreeNode.Selected = true;
        }

        void cmdSelectDataSource_Click(object sender, EventArgs e)
        {
            var datasource = GetDatasource();

            var selection = from node in treeList1.Selection.OfType<TreeListNode>()
                            let index = node.Id
                            let treeItem = datasource[index]
                            where treeItem.IsDatasourceAvailable == true
                            where treeItem.IsStructure == false
                            select treeItem;

            if (selection.Count() != 1)
            {
                MessageBox.Show("Please select one Datasouce, Dan. Thanks!");
                return;
            }

            var definition = selection.First().DesignTimeDataSourceDefinition;
            _callback.Invoke(definition);
        }

        List<DesignTimeDataSourceTreeItem> GetDatasource()
        {
            return (bindingSource1.DataSource as IEnumerable<DesignTimeDataSourceTreeItem>).ToList();
        }

    }
}
