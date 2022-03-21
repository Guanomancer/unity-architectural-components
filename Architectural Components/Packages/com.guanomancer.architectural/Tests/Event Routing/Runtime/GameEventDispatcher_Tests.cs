using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Guanomancer;

namespace Guanomancer.EventRouting.TestsPlayMode
{
    public class GameEventDispatcher_Tests : IGameEventSubscriber
    {
        private GameEvent _start, _stop;
        private GameEventDispatcher _dispatcher;

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
            _dispatcher = new GameObject("Game Event Controller Object").AddComponent<GameEventDispatcher>();
            _dispatcher.SetPrivateFieldsInTest(new (string, object)[] {
                ("_gameEvent", _start),
                ("_contextType", GameEventContextType.Integer),
                ("_integerContext", 1337),
                });
        }

        [UnityTest]
        public IEnumerator Dispatch_FiresEvents()
        {
            var value = 0;
            _start.Subscribe(this, (sender, context) => { value = (int)context; });

            _dispatcher.Dispatch();

            Assert.AreEqual(1337, value);

            yield return null;
        }
    }
}
