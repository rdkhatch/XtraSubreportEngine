using System.Linq;
using DevExpress.XtraReports.UserDesigner;
using XtraSubreport.Engine.Designer;

namespace XtraSubreport.Engine
{
    public class PreviewCommandHandler : ICommandHandler
    {
        private readonly XRDesignPanel _designPanel;
        private readonly IDesignerContext _designerContext;

        public PreviewCommandHandler(XRDesignPanel designPanel, IDesignerContext designerContext)
        {
            _designPanel = designPanel;
            _designerContext = designerContext;
        }

        public void HandleCommand(ReportCommand command, object[] args, ref bool handled)
        {
            var report = _designPanel.Report;
            var controller = _designerContext.GetController(report);
            
            if (command == ReportCommand.ShowPreviewTab)
            {
                var newReport = controller.Print(r => r.ShowPreviewDialog(_designPanel.LookAndFeel));
            }
        }

        public bool CanHandleCommand(ReportCommand command)
        {
            return command == ReportCommand.ShowDesignerTab;
        }
    }
}