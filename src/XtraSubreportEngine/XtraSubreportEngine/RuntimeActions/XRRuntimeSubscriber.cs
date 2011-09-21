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
            // Event handlers DO NOT WORK in the end user designer - because of DevExpress serialization / CodeDom
            // http://www1.devexpress.com/Support/Center/p/Q256674.aspx
            // http://www.devexpress.com/Support/Center/p/Q240047.aspx
            // Must either:
            // 1.) Override Before_Print within custom reports class (ie, MyReportBase)
            // 2.) Use Scripts create an event handler
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