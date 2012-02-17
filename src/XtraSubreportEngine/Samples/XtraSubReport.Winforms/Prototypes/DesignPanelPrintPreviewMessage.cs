using System;
using System.Linq;
using DevExpress.XtraReports.UserDesigner;

namespace XtraSubReport.Winforms.Prototypes
{
    public class DesignPanelPrintPreviewMessage
    {
        public XRDesignPanel DesignPanel { get; set; }

        public DesignPanelPrintPreviewMessage(XRDesignPanel designPanel)
        {
            DesignPanel = designPanel;
        }
    }
}