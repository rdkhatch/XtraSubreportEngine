using DevExpress.XtraReports.UserDesigner;

namespace XtraSubreport.Engine.Designer
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
            // Prevent Preview Tab from being shown
            handled = true;

            var designReport = _designPanel.Report;

            // Must clone design report.  Cannot preview same report instance while it's being designed.
            // This also converts to our base class - which will fire BeforePrint events
            var previewReport = designReport.CloneLayoutAsMyReportBase();

            var reportController = _designerContext.GetController(previewReport);

            // Print Preview!
            reportController.Print(r => r.ShowPreviewDialog(_designPanel.LookAndFeel));
        }

        public bool CanHandleCommand(ReportCommand command)
        {
            return command == ReportCommand.ShowPreviewTab;
        }
    }
}