using System;
using System.Linq;
using DevExpress.XtraReports.UserDesigner;

namespace XtraSubReport.Winforms.Prototypes
{
    public class RelayCommandHandler : ICommandHandler
    {
        private readonly Func<ReportCommand, object[], bool> _commandHandlerFunc;
        private readonly Func<ReportCommand, bool> _canHandleAction;


        public RelayCommandHandler(Func<ReportCommand, object[], bool> commandHandlerFunc, Func<ReportCommand, bool> canHandleAction)
        {
            _commandHandlerFunc = commandHandlerFunc;
            _canHandleAction = canHandleAction;
            _commandHandlerFunc = commandHandlerFunc;
        }

        public void HandleCommand(ReportCommand command, object[] args, ref bool handled)
        {
            handled = _commandHandlerFunc(command, args);
        }

        public bool CanHandleCommand(ReportCommand command)
        {
            return _canHandleAction(command);
        }
    }
}