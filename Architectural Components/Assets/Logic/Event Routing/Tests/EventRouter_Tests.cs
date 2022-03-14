using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Guanomancer.EventRouting.Tests
{
    public class EventRouter_Tests : IEventSubscriber
    {
        private Queue<IEventContext> _eventQueue = new Queue<IEventContext>();

        public void OnEvent(IEventContext context) => _eventQueue.Enqueue(context);

        [SetUp]
        public void Setup()
        {
            EventRouter.UnsubscribeAll(this);
        }

        [Test]
        public void SubscribeByT_EnablesReceivingEvents()
        {
            EventRouter<MyEventA>.Subscribe(this);

            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(1, _eventQueue.Count);
            Assert.AreEqual(typeof(MyEventA), _eventQueue.Dequeue().GetType());
        }

        [Test]
        public void SubscribeByType_EnablesReceivingEvents()
        {
            EventRouter.Subscribe(typeof(MyEventA), this);

            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(1, _eventQueue.Count);
            Assert.AreEqual(typeof(MyEventA), _eventQueue.Dequeue().GetType());
        }

        [Test]
        public void SubscribeByName_EnablesReceivingEvents()
        {
            EventRouter.Subscribe(nameof(MyEventA), this);

            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(1, _eventQueue.Count);
            Assert.AreEqual(typeof(MyEventA), _eventQueue.Dequeue().GetType());
        }

        [Test]
        public void SubscribeByT_ReceivesOnlySubscribedEventTypes()
        {
            EventRouter<MyEventA>.Subscribe(this);

            EventRouter.Dispatch(new MyEventB { });
            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(typeof(MyEventA), _eventQueue.Dequeue().GetType());
            Assert.AreEqual(0, _eventQueue.Count);
        }

        [Test]
        public void SubscribeByType_ReceivesOnlySubscribedEventTypes()
        {
            EventRouter.Subscribe(typeof(MyEventA), this);

            EventRouter.Dispatch(new MyEventB { });
            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(typeof(MyEventA), _eventQueue.Dequeue().GetType());
            Assert.AreEqual(0, _eventQueue.Count);
        }

        [Test]
        public void SubscribeByName_ReceivesOnlySubscribedEventTypes()
        {
            EventRouter.Subscribe(nameof(MyEventA), this);

            EventRouter.Dispatch(new MyEventB { });
            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(typeof(MyEventA), _eventQueue.Dequeue().GetType());
            Assert.AreEqual(0, _eventQueue.Count);
        }

        [Test]
        public void UnsubscribeByT_StopsReceivingSpecificEventType()
        {
            EventRouter<MyEventA>.Subscribe(this);
            EventRouter<MyEventB>.Subscribe(this);
            EventRouter<MyEventA>.Unsubscribe(this);

            EventRouter.Dispatch(new MyEventB { });
            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(typeof(MyEventB), _eventQueue.Dequeue().GetType());
            Assert.AreEqual(0, _eventQueue.Count);
        }

        [Test]
        public void UnsubscribeByType_StopsReceivingSpecificEventType()
        {
            EventRouter.Subscribe(typeof(MyEventA), this);
            EventRouter.Subscribe(typeof(MyEventB), this);
            EventRouter.Unsubscribe(typeof(MyEventA), this);

            EventRouter.Dispatch(new MyEventB { });
            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(typeof(MyEventB), _eventQueue.Dequeue().GetType());
            Assert.AreEqual(0, _eventQueue.Count);
        }

        [Test]
        public void UnsubscribeByName_StopsReceivingSpecificEventType()
        {
            EventRouter.Subscribe(nameof(MyEventA), this);
            EventRouter.Subscribe(nameof(MyEventB), this);
            EventRouter.Unsubscribe(nameof(MyEventA), this);

            EventRouter.Dispatch(new MyEventB { });
            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(typeof(MyEventB), _eventQueue.Dequeue().GetType());
            Assert.AreEqual(0, _eventQueue.Count);
        }

        [Test]
        public void UnsubscribeAll_StopsReceivingAllEventTypes()
        {
            EventRouter<MyEventA>.Subscribe(this);
            EventRouter<MyEventB>.Subscribe(this);
            EventRouter.UnsubscribeAll(this);

            EventRouter.Dispatch(new MyEventB { });
            EventRouter.Dispatch(new MyEventA { });

            Assert.AreEqual(0, _eventQueue.Count);
        }
    }

    public struct MyEventA : IEventContext { }
    public struct MyEventB : IEventContext { }

    public struct MyEventC : IEventContext { }
}
