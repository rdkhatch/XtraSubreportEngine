namespace XtraSubReport.Winforms.Popups
{
    partial class NoProjectsExistWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoProjectsExistWarning));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(38, 21);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(333, 23);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "No projects have been created yet.";
            // 
            // memoEdit1
            // 
            this.memoEdit1.EditValue = resources.GetString("memoEdit1.EditValue");
            this.memoEdit1.Location = new System.Drawing.Point(21, 59);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.ReadOnly = true;
            this.memoEdit1.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.memoEdit1.Size = new System.Drawing.Size(363, 142);
            this.memoEdit1.TabIndex = 4;
            // 
            // NoProjectsExistWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 214);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NoProjectsExistWarning";
            this.Text = "Cannot Continue";
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
    }
}