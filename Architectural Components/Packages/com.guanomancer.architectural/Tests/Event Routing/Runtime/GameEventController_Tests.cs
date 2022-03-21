using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Guanomancer;

namespace Guanomancer.EventRouting.TestsPlayMode
{
    public class GameEventController_Tests : IGameEventSubscriber
    {
        private GameEvent _start, _stop;
        private GameEventController _controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _start = ScriptableObject.CreateInstance<GameEvent>();
            _start.name = "Start";
            _stop = ScriptableObject.CreateInstance<GameEvent>();
            _stop.name = "Stop";
        }

        [SetUp]
        public void Setup()
        {
        }

        [UnityTest]
        public IEnumerator Dispatch_FiresEvents()
        {
            var count = 0;
            var valueStart = 0f;
            var unityEventStart = new UnityEngine.Events.UnityEvent<object, object>();
            unityEventStart.AddListener((sender, context) => { valueStart = (float)context; count++; });
            var valueStop = false;
            var unityEventStop = new UnityEngine.Events.UnityEvent<object, object>();
            unityEventStop.AddListener((sender, context) => { valueStop = (bool)context; count++; });
            _controller = new GameObject("Game Event Controller Object").AddComponent<GameEventController>();
            _controller.SetPrivateFieldInTest("_events", new GameEventControllerEntry[] {
                new GameEventControllerEntry { GameEvent = _start, UnityEvent = unityEventStart },
                new GameEventControllerEntry { GameEvent = _stop, UnityEvent = unityEventStop },
                });
            _controller.OnEnable();

            _start.Dispatch(this, 3.14f);
            _stop.Dispatch(this, true);

            Assert.AreEqual(2, count);
            Assert.AreEqual(3.14f, valueStart);
            Assert.AreEqual(true, valueStop);

            yield return null;
        }
    }
}
