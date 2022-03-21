using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Guanomancer.EventRouting.Tests
{
    public class GameEvent_Tests : IGameEventSubscriber
    {
        private GameEvent _start, _stop;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _start = ScriptableObject.CreateInstance<GameEvent>();
            _start.name = "Start";
            _stop = ScriptableObject.CreateInstance<GameEvent>();
            _stop.name = "Stop";
        }

        [TearDown]
        public void TearDown()
        {
            _start.Unsubscribe(this);
            _stop.Unsubscribe(this);
        }

        [Test]
        public void Subscribe_AddsHandler()
        {
            _start.Subscribe(this, (sender, context) => { });

            Assert.IsTrue(_start.IsSubscribed(this));
        }

        [Test]
        public void Unsubscribe_RemovesHandler()
        {
            _start.Subscribe(this, (sender, context) => { });
            _start.Unsubscribe(this);

            Assert.IsFalse(_start.IsSubscribed(this));
        }

        [Test]
        public void Dispatch_NotifiesAllHandlers()
        {
            var count = 0;
            var i = 0;
            var b = false;
            _start.Subscribe(this, (sender, context) => { count++; i = (int)context; });
            _stop.Subscribe(this, (sender, context) => { count++; b = (bool)context; });

            _start.Dispatch(this, 1337);
            _stop.Dispatch(this, true);

            Assert.AreEqual(2, count);
            Assert.AreEqual(1337, i);
            Assert.AreEqual(true, b);
        }
    }
}
