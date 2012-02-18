using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NLog;
using NLog.Targets;
using XtraSubreport.Engine.Eventing;

namespace XtraSubReport.Winforms.Support
{
    [Target("MessagePublishingTarget")]
    public sealed class MessagePublishingTarget : TargetWithLayout
    {       
        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = Layout.Render(logEvent);
            EventAggregator.Singleton.Publish(new NLogMessage(logEvent));
        }
    }
}
