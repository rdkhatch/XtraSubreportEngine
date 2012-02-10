using System;
using System.Collections.Generic;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.Eventing;

namespace XtraSubreport.Engine.RuntimeActions
{
    public class GlobalXRSubscriber : IHandle<XRBeforePrintMessage>
    {
        public static GlobalXRSubscriber Singleton { get; private set; }      

        private readonly IEventAggregator _aggregator;

        public static void Init()
        {
            if(Singleton == null)
                Singleton = new GlobalXRSubscriber(EventAggregator.Singleton);
        }

        private GlobalXRSubscriber(IEventAggregator aggregator)
        {
            _aggregator = aggregator;

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
            VisitMethodRecursively(message);
        }

        public readonly Dictionary<int,WeakReference> Visitors = new Dictionary<int, WeakReference>();

        private void VisitMethodRecursively(XRBeforePrintMessage message)
        {
            var incomingHashcode = message.Report.GetHashCode();

            // prevent multiple visitors on the same report.
            // a report can print multiple times if it is a subreport
            if (Visitors.ContainsKey(incomingHashcode)) return;

            using (var visitor = new XRRuntimeVisitor(_aggregator, message.Report)) 
            {
                Visitors.Add(incomingHashcode, new WeakReference(visitor));
                visitor.Visit();
            }
        }

    }
}