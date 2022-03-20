using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Guanomancer.StateMachines.TestsPlayMode
{
    public class StateMachine_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [UnityTest]
        public IEnumerator StateMachine_ProgressThroughTransitions()
        {
            var gameObject = new GameObject();
            var fsm = gameObject.AddComponent<StateMachine>();
            var start = gameObject.AddComponent<StartState>();
            var end = gameObject.AddComponent<EndState>();

            Assert.IsNull(fsm.StateName);

            yield return null;

            Assert.IsTrue(fsm.IsState<EndState>());

            yield return null;

            Assert.IsTrue(fsm.IsState<EndState>());
        }
    }
}
