using System.Collections.Generic;
using System.Linq;
using XtraSubreport.Engine.Eventing;

namespace XtraSubReport.Winforms.Prototypes
{
    public class MessageHandler : IHandle<ReportActivatedMessage>, IHandle<ReportActivatedBySubreportMessage>, IHandle<DesignPanelPrintPreviewMessage>
    {
        private int _counter = 1;
        public MessageHandler(IEventAggregator aggregator)
        {
            _messages = new List<MessageInfo>();
            aggregator.Subscribe(this);
        }

        public IEnumerable<MessageInfo> GetMessageInfos()
        {
            return _messages.ToArray();
        }

        private readonly List<MessageInfo> _messages;

        private void AddMessage(object message)
        {
            var messageInfo = new MessageInfo(message, _counter);
            _counter++;

            _messages.Add(messageInfo);
        }

        public void Handle(ReportActivatedMessage message)
        {
            AddMessage(message);
        }

        public void Handle(ReportActivatedBySubreportMessage message)
        {
            AddMessage(message);
        }

        public void Handle(DesignPanelPrintPreviewMessage message)
        {
            AddMessage(message);
        }
    }
}