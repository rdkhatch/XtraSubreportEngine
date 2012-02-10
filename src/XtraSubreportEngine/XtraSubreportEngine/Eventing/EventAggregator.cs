﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XtraSubreport.Engine.Eventing
{
    public partial class EventAggregator
    {
        public static IEventAggregator Singleton { get; private set; }

        static EventAggregator()
        {
            Singleton = new EventAggregator();
        }
    }

}

// Taken from Caliburn.Micro
namespace XtraSubreport.Engine.Eventing
{
    /// <summary>
    ///   A marker interface for classes that subscribe to messages.
    /// </summary>
    public interface IHandle { }

    /// <summary>
    ///   Denotes a class which can handle a particular type of message.
    /// </summary>
    /// <typeparam name = "TMessage">The type of message to handle.</typeparam>
    public interface IHandle<TMessage> : IHandle
    {
        /// <summary>
        ///   Handles the message.
        /// </summary>
        /// <param name = "message">The message.</param>
        void Handle(TMessage message);
    }

    /// <summary>
    ///   Enables loosely-coupled publication of and subscription to events.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        ///   Gets or sets the default publication thread marshaller.
        /// </summary>
        /// <value>
        ///   The default publication thread marshaller.
        /// </value>
        Action<System.Action> PublicationThreadMarshaller { get; set; }

        /// <summary>
        ///   Subscribes an instance to all events declared through implementations of <see cref = "IHandle{T}" />
        /// </summary>
        /// <param name = "instance">The instance to subscribe for event publication.</param>
        void Subscribe(object instance);

        /// <summary>
        ///   Unsubscribes the instance from all events.
        /// </summary>
        /// <param name = "instance">The instance to unsubscribe.</param>
        void Unsubscribe(object instance);

        /// <summary>
        ///   Publishes a message.
        /// </summary>
        /// <param name = "message">The message instance.</param>
        /// <remarks>
        ///   Uses the default thread marshaller during publication.
        /// </remarks>
        void Publish(object message);

        /// <summary>
        ///   Publishes a message.
        /// </summary>
        /// <param name = "message">The message instance.</param>
        /// <param name = "marshal">Allows the publisher to provide a custom thread marshaller for the message publication.</param>
        void Publish(object message, Action<System.Action> marshal);
    }

    /// <summary>
    ///   Enables loosely-coupled publication of and subscription to events.
    /// </summary>
    public partial class EventAggregator : IEventAggregator
    {
        /// <summary>
        ///   The default thread marshaller used for publication;
        /// </summary>
        public static Action<System.Action> DefaultPublicationThreadMarshaller = action => action();

        readonly List<Handler> _handlers = new List<Handler>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "EventAggregator" /> class.
        /// </summary>
        public EventAggregator()
        {
            PublicationThreadMarshaller = DefaultPublicationThreadMarshaller;
        }

        /// <summary>
        ///   Gets or sets the default publication thread marshaller.
        /// </summary>
        /// <value>
        ///   The default publication thread marshaller.
        /// </value>
        public Action<System.Action> PublicationThreadMarshaller { get; set; }

        /// <summary>
        ///   Subscribes an instance to all events declared through implementations of <see cref = "IHandle{T}" />
        /// </summary>
        /// <param name = "instance">The instance to subscribe for event publication.</param>
        public virtual void Subscribe(object instance)
        {
            lock (_handlers)
            {
                if (_handlers.Any(x => x.Matches(instance)))
                    return;

                _handlers.Add(new Handler(instance));
            }
        }

        /// <summary>
        ///   Unsubscribes the instance from all events.
        /// </summary>
        /// <param name = "instance">The instance to unsubscribe.</param>
        public virtual void Unsubscribe(object instance)
        {
            lock (_handlers)
            {
                var found = _handlers.FirstOrDefault(x => x.Matches(instance));

                if (found != null)
                    _handlers.Remove(found);
            }
        }

        /// <summary>
        ///   Publishes a message.
        /// </summary>
        /// <param name = "message">The message instance.</param>
        /// <remarks>
        ///   Does not marshall the the publication to any special thread by default.
        /// </remarks>
        public virtual void Publish(object message)
        {
            Publish(message, PublicationThreadMarshaller);
        }

        /// <summary>
        ///   Publishes a message.
        /// </summary>
        /// <param name = "message">The message instance.</param>
        /// <param name = "marshal">Allows the publisher to provide a custom thread marshaller for the message publication.</param>
        public virtual void Publish(object message, Action<Action> marshal)
        {
            Handler[] toNotify;
            lock (_handlers)
                toNotify = _handlers.ToArray();

            marshal(() =>
            {
                var messageType = message.GetType();

                var dead = toNotify
                    .Where(handler => !handler.Handle(messageType, message))
                    .ToList();

                if (dead.Any())
                {
                    lock (_handlers)
                    {
                        dead.ForEach(x => _handlers.Remove(x));
                    }
                }
            });
        }

        protected class Handler
        {
            readonly WeakReference _reference;
            readonly Dictionary<Type, MethodInfo> _supportedHandlers = new Dictionary<Type, MethodInfo>();

            public Handler(object handler)
            {
                _reference = new WeakReference(handler);

                var interfaces = handler.GetType().GetInterfaces()
                    .Where(x => typeof(IHandle).IsAssignableFrom(x) && x.IsGenericType);

                foreach (var @interface in interfaces)
                {
                    var type = @interface.GetGenericArguments()[0];
                    var method = @interface.GetMethod("Handle");
                    _supportedHandlers[type] = method;
                }
            }

            public bool Matches(object instance)
            {
                return _reference.Target == instance;
            }

            public bool Handle(Type messageType, object message)
            {
                var target = _reference.Target;
                if (target == null)
                    return false;

                foreach (var pair in _supportedHandlers)
                {
                    if (pair.Key.IsAssignableFrom(messageType))
                    {
                        pair.Value.Invoke(target, new[] { message });
                        return true;
                    }
                }

                return true;
            }
        }
    }

}

