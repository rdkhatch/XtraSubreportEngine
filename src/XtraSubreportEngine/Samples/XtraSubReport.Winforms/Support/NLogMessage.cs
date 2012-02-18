using System;
using System.Linq;
using NLog;

namespace XtraSubReport.Winforms.Support
{
    public class NLogMessage
    {
        public LogEventInfo LogMessage { get; set; }

        public NLogMessage(LogEventInfo logMessage)
        {
            LogMessage = logMessage;
        }
    }
}