using System.Linq;
using DevExpress.XtraReports.UserDesigner;

namespace XtraSubReport.Winforms.Prototypes
{
    public class DesignPanelPrintPreviewMessage
    {
        public XRDesignPanel Sender { get; set; }

        public DesignPanelPrintPreviewMessage(XRDesignPanel sender)
        {
            Sender = sender;
        }
    }
}