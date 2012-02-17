using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XtraSubReport.Winforms.Support;

namespace XtraSubReport.Winforms.Popups
{
    public partial class ChooseProject : Form
    {
        private readonly AppBootStrapper _bootStrapper;

        public ChooseProject()
        {
            InitializeComponent();
        }

        private void acceptAndContinueBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var item = multipleProjectsListBoxControl.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(item))
            {
                MessageBox.Show("Please retry", "Item was not selected, please try again");
                return;
            }

            try
            {
                _bootStrapper.SetProjectName(item);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", "An error has happened: \r\n" + ex.Message, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            }
            

        }

        public ChooseProject(AppBootStrapper bootStrapper) : this()
        {
            _bootStrapper = bootStrapper;

            this.multipleProjectsListBoxControl.DataSource = _bootStrapper.GetProjects().ToArray();

        }
    }
}
