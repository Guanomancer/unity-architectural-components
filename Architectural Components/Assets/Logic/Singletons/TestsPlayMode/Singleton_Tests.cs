using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Guanomancer.Singletons.TestsPlayMode
{
    public class Singleton_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [UnityTest]
        public IEnumerator Instance_IsAvailableAfterAwake()
        {
            Assert.IsNull(MySingleton.Instance);

            var gameObject = new GameObject();
            var single = gameObject.AddComponent<MySingleton>();

            Assert.IsNotNull(MySingleton.Instance);

            yield return null;
        }
    }

    public class MySingleton : SingletonBehaviour<MySingleton>
    {
        public override bool PersistThroughSceneChange => false;
        public override bool HideInInspector => false;
        public override bool HideInHierarchy => false;
    }
}
