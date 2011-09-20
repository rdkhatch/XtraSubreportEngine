using XtraSubreport.Contracts.RuntimeActions;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class XRRuntimeSubscriber : IHandle<XRBeforePrintMessage>
    {
        private IRuntimeActionController _controller;

        public XRRuntimeSubscriber(IRuntimeActionController controller)
            : this(controller, EventAggregator.Singleton)
        {
        }

        public XRRuntimeSubscriber(IRuntimeActionController controller, IEventAggregator aggregator)
        {
            _controller = controller;

            // Attach to Event Aggregator
            // Allows this Visitor to listen to any MyReportBase's BeforePrint event.
            // Important because DevExpress uses CodeDom and serialization for report scripting etc.  Attaching to BeforePrint events will not work.  You must attach to the Aggregator instead.
            aggregator.Subscribe(this);
        }

        void IHandle<XRBeforePrintMessage>.Handle(XRBeforePrintMessage message)
        {
            var visitor = new XRRuntimeVisitor(_controller);
            visitor.Visit(message.Report);
        }

        public static XRRuntimeSubscriber SubscribeWithActions(params IReportRuntimeAction[] actions)
        {
            var controller = new XRRuntimeActionController(actions);
            return new XRRuntimeSubscriber(controller);
        }
    }
}