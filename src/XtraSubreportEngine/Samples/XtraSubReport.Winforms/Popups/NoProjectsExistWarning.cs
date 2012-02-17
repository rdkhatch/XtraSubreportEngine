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
    public partial class NoProjectsExistWarning : Form
    {
        private readonly AppBootStrapper _bootStrapper;

        public NoProjectsExistWarning()
        {
            InitializeComponent();
        }

        public NoProjectsExistWarning(AppBootStrapper bootStrapper) : this()
        {
            _bootStrapper = bootStrapper;

            memoEdit1.Text = memoEdit1.Text.Replace("@PATH", bootStrapper.RootPath);
        }
    }
}
