using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using XtraSubreport.Engine.Eventing;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class ScopedXRSubscriber : IHandle<ScopedXRControlBeforePrintMessage>, IDisposable
    {
        private readonly IEventAggregator _aggregator;
        private readonly int _rootHashCode;
        private readonly Action<XRControl> _handler;

        public ScopedXRSubscriber(int rootHashCode, Action<XRControl> handler) : this(EventAggregator.Singleton, rootHashCode, handler)
        {
        }

        public ScopedXRSubscriber(IEventAggregator aggregator, int rootHashCode, Action<XRControl> handler)
        {
            _aggregator = aggregator;
            _rootHashCode = rootHashCode;
            _handler = handler;

            _aggregator.Subscribe(this);
        }

        public void Handle(ScopedXRControlBeforePrintMessage message)
        {
            if (message.RootReportHashcode == _rootHashCode)
            {
                _handler(message.Control);
            }
        }

        public void Dispose()
        {
            _aggregator.Unsubscribe(this);
        }
    }
}