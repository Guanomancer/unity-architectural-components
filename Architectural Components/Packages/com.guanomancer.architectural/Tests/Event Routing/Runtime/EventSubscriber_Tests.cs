using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Guanomancer.EventRouting.TestsPlayMode
{
    public class EventController_Tests
    {
        private Queue<MyEventC> _aQueue = new Queue<MyEventC>();
        private void OnMyEventC(IEventContext context) => _aQueue.Enqueue((MyEventC)context);

        private Queue<MyEventD> _bQueue = new Queue<MyEventD>();
        private void OnMyEventD(IEventContext context) => _bQueue.Enqueue((MyEventD)context);

        [SetUp]
        public void Setup()
        {
            EventRouter.LogEventsByDefault = true;
            _aQueue.Clear();
            _bQueue.Clear();
        }

        [UnityTest]
        public IEnumerator EventController_RegistersAndFiresEvents()
        {
            var gameObject = new GameObject();
            var controller = gameObject.AddComponent<EventSubscriber>();
            controller.Bindings = new List<EventBinding>();
            var binding = new EventBinding { EventName = nameof(MyEventC) };
            binding.OnEvent = new UnityEngine.Events.UnityEvent<IEventContext>();
            binding.OnEvent.AddListener(OnMyEventC);
            controller.Bindings.Add(binding);
            controller.OnEnable();

            EventRouter.Dispatch(new MyEventC { });

            controller.OnDisable();

            Assert.AreEqual(1, _aQueue.Count);

            yield return null;
        }

        [UnityTest]
        public IEnumerator EventController_RegistersAndFiresCorrectEvents()
        {
            var gameObject = new GameObject();
            var controller = gameObject.AddComponent<EventSubscriber>();
            controller.Bindings = new List<EventBinding>();
            var bindingC = new EventBinding { EventName = nameof(MyEventC) };
            bindingC.OnEvent = new UnityEngine.Events.UnityEvent<IEventContext>();
            bindingC.OnEvent.AddListener(OnMyEventC);
            controller.Bindings.Add(bindingC);
            controller.OnEnable();

            EventRouter.Dispatch(new MyEventC { });
            EventRouter.Dispatch(new MyEventD { });

            controller.OnDisable();

            Assert.AreEqual(1, _aQueue.Count);
            Assert.AreEqual(typeof(MyEventC), _aQueue.Dequeue().GetType());

            yield return null;
        }
    }

    public struct MyEventC : IEventContext { }
    public struct MyEventD : IEventContext { }
}
