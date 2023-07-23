using Messaging.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Messaging
{
    public class MessageBus : Singleton<MessageBus>
        ,IMessageBus
    {
        /// <summary>
        /// Collection containing all subscribers for a type of message.
        /// </summary>
        private readonly Dictionary<Type, List<WeakReference>> _subscribers = new();
        private readonly object _lockObject = new();

        /// <summary>
        /// Publish a message.
        /// </summary>
        public void Publish<T>(T message) where T : IMessage
        {
            Type subscriberType = typeof(ISubscriber<>).MakeGenericType(typeof(T));

            List<WeakReference> subscribers = GetSubscriberList(subscriberType);
            List<WeakReference> subsToRemove = new();

            for (int i = 0; i < subscribers.Count; i++)
            {
                WeakReference weakSubscriber = subscribers[i];
                ISubscriber<T> subscriber = (ISubscriber<T>)weakSubscriber.Target;
                
                if (weakSubscriber.IsAlive == false || subscriber == null || subscriber.Equals(null))
                {
                    subsToRemove.Add(weakSubscriber);
                    continue;
                }

                InvokeSubscriberEvent(message, subscriber);
            }

            if (subsToRemove.Count <= 0)
            {
                return;
            }

            lock (_lockObject)
            {
                foreach (WeakReference remove in subsToRemove)
                {
                    subscribers.Remove(remove);
                }
            }
        }

        /// <summary>
        /// Subscribe a object to receive messages.
        /// </summary>
        public void Subscribe(object subscriber)
        {
            lock (_lockObject)
            {
                IEnumerable<Type> subscriberTypes =
                    subscriber.GetType()
                        .GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

                WeakReference weakRef = new(subscriber);

                foreach (Type subscriberType in subscriberTypes)
                {
                    List<WeakReference> subscribers = GetSubscriberList(subscriberType);
                    
                    if (subscribers.Any(x => x.Target == weakRef.Target))
                    {
                        continue;
                    }

                    subscribers.Add(weakRef);
                }
            }
        }

        private static void InvokeSubscriberEvent<T>(T message, ISubscriber<T> subscriber) where T : IMessage
            => subscriber?.OnMessage(message);

        private List<WeakReference> GetSubscriberList(Type subscriberType)
        {
            List<WeakReference> subscribersList;

            lock (_lockObject)
            {
                bool found = _subscribers.TryGetValue(subscriberType, out subscribersList);

                if (found)
                {
                    return subscribersList;
                }

                subscribersList = new List<WeakReference>();
                _subscribers.Add(subscriberType, subscribersList);
            }

            return subscribersList;
        }
    }
}

