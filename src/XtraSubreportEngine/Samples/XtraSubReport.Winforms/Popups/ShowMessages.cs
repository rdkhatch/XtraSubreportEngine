using System.Linq;
using System.Windows.Forms;
using XtraSubReport.Winforms.Prototypes;

namespace XtraSubReport.Winforms.Popups
{
    public partial class ShowMessages : Form
    {
        public ShowMessages()
        {
            InitializeComponent();
        }

        public ShowMessages(MessageHandler debugMessageHandler)
        {
            InitializeComponent();
            this.messageInfoBindingSource.DataSource = debugMessageHandler.GetMessageInfos();
        }
    }
}
