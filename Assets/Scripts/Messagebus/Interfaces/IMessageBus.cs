using System.Collections;
using System.Collections.Generic;

namespace Messaging.Interfaces
{
    /// <summary>
    /// The messageBus acts as a intermediary between systems of the game, game-systems can easily register with the message bus and publish messages to each other.
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Publish a message.
        /// </summary>
        void Publish<T>(T message) where T : IMessage;

        /// <summary>
        /// Subscribe a object to receive messages for all implementations of ISubscriber.
        /// </summary>
        void Subscribe(object subscriber);
    }
}
