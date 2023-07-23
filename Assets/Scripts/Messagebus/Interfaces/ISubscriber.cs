namespace Messaging.Interfaces
{
    /// <summary>
    /// Interface that is used to identify which message a object wants to receive.
    /// </summary>
    public interface ISubscriber<T> where T : IMessage
    {
        /// <summary>
        /// Method invoked when a message of type T is received.
        /// </summary>
        void OnMessage(T message);
    }
}
