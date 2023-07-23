using System;
using Messaging.Interfaces;
using Messaging;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Assertions;
using Assert = NUnit.Framework.Assert;

#if UNITY_EDITOR
namespace Messaging.test
{
    public class MessageBusTests
    {
        public class TestMessage : IMessage
        {
        }

        public class AnotherTestMessage : IMessage
        {
        }

        public class MessageListener : MonoBehaviour, ISubscriber<TestMessage>
        {
            public TestMessage receivedMessage;
            public int receivedAmount;

            public void OnMessage(TestMessage message)
            {
                receivedMessage = message;
                receivedAmount++;
            }
        }

        [Test]
        public void Subscribe_Once_EventReceivedAfterPublish()
        {
            // Arrange.
            MessageBus sut = new MessageBus();
            TestMessage message = new TestMessage();
            MessageListener listener = new GameObject("MessageListener", typeof(MessageListener)).GetComponent<MessageListener>();

            sut.Subscribe(listener);

            // Act.
            sut.Publish(message);

            // Assert.
            Assert.AreSame(message, listener.receivedMessage);
            Assert.AreEqual(1, listener.receivedAmount);
        }

        [Test]
        public void Subscribe_Multiple_EventReceivedAfterPublish()
        {
            // Arrange.
            MessageBus sut = new MessageBus();
            TestMessage message = new TestMessage();
            MessageListener listener = new GameObject("MessageListener", typeof(MessageListener)).GetComponent<MessageListener>();

            sut.Subscribe(listener);
            sut.Subscribe(listener);

            // Act.
            sut.Publish(message);

            // Assert.
            Assert.AreSame(message, listener.receivedMessage);
            Assert.AreEqual(1, listener.receivedAmount);
        }

        [Test]
        public void Subscribe_OtherMessage_EventNotReceivedAfterPublish()
        {
            // Arrange.
            MessageBus sut = new MessageBus();
            AnotherTestMessage message = new AnotherTestMessage();
            MessageListener listener = new GameObject("MessageListener", typeof(MessageListener)).GetComponent<MessageListener>();

            sut.Subscribe(listener);

            // Act.
            sut.Publish(message);

            // Assert.
            Assert.IsNull(listener.receivedMessage);
            Assert.AreEqual(0, listener.receivedAmount);
        }

        [Test]
        public void Subscribe_Message_NotReceivedAfterDestroy()
        {
            // Arrange.
            MessageBus sut = new MessageBus();
            TestMessage message = new TestMessage();
            MessageListener listener = new GameObject("MessageListener", typeof(MessageListener)).GetComponent<MessageListener>();

            sut.Subscribe(listener);
            UnityEngine.Object.DestroyImmediate(listener.gameObject);

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert.
            Assert.DoesNotThrow(() => { sut.Publish(message); });
        }
    }
}
#endif