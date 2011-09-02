namespace XtraSubreport.Engine
{
    partial class SelectDesignTimeDataSourceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectDesignTimeDataSourceForm));
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.colDataSourceName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDataSourceRelationPath = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cmdSelectDataSource = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colDataSourceName,
            this.colDataSourceRelationPath,
            this.treeListColumn1,
            this.treeListColumn2});
            this.treeList1.DataSource = this.bindingSource1;
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.OptionsBehavior.ImmediateEditor = false;
            this.treeList1.OptionsLayout.AddNewColumns = false;
            this.treeList1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeList1.SelectImageList = this.imageList1;
            this.treeList1.Size = new System.Drawing.Size(824, 373);
            this.treeList1.StateImageList = this.imageList1;
            this.treeList1.TabIndex = 0;
            // 
            // colDataSourceName
            // 
            this.colDataSourceName.Caption = "Name";
            this.colDataSourceName.FieldName = "Name";
            this.colDataSourceName.MinWidth = 49;
            this.colDataSourceName.Name = "colDataSourceName";
            this.colDataSourceName.Visible = true;
            this.colDataSourceName.VisibleIndex = 0;
            this.colDataSourceName.Width = 214;
            // 
            // colDataSourceRelationPath
            // 
            this.colDataSourceRelationPath.FieldName = "RelationPath";
            this.colDataSourceRelationPath.Name = "colDataSourceRelationPath";
            this.colDataSourceRelationPath.Visible = true;
            this.colDataSourceRelationPath.VisibleIndex = 1;
            this.colDataSourceRelationPath.Width = 217;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.FieldName = "PreviouslyUsedWithThisReport";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 3;
            this.treeListColumn1.Width = 179;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.FieldName = "IsDatasourceAvailable";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 2;
            this.treeListColumn2.Width = 197;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(XtraSubreport.Engine.Support.DesignTimeDataSourceTreeItem);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder.ico");
            this.imageList1.Images.SetKeyName(1, "Data_Dataset.ico");
            this.imageList1.Images.SetKeyName(2, "Data_Dataset.NotAvailable.png");
            // 
            // cmdSelectDataSource
            // 
            this.cmdSelectDataSource.Location = new System.Drawing.Point(337, 384);
            this.cmdSelectDataSource.Name = "cmdSelectDataSource";
            this.cmdSelectDataSource.Size = new System.Drawing.Size(161, 23);
            this.cmdSelectDataSource.TabIndex = 1;
            this.cmdSelectDataSource.Text = "Select Data Source";
            this.cmdSelectDataSource.UseVisualStyleBackColor = true;
            this.cmdSelectDataSource.Click += new System.EventHandler(this.cmdSelectDataSource_Click);
            // 
            // SelectDesignTimeDataSourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 419);
            this.Controls.Add(this.cmdSelectDataSource);
            this.Controls.Add(this.treeList1);
            this.Name = "SelectDesignTimeDataSourceForm";
            this.Text = "Select Design Time Data Source";
            this.Load += new System.EventHandler(this.SelectDesignTimeDataSourceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeList1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDataSourceName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDataSourceRelationPath;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button cmdSelectDataSource;

    }
}