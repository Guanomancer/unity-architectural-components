using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Guanomancer.Singletons.TestsPlayMode
{
    public class GuranteedAvailabilitySingleton_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [UnityTest]
        public IEnumerator Instance_IsAlwaysAvailable()
        {
            Assert.IsNotNull(MyGuranteedSingleton.Instance);

            yield return null;
        }
    }

    public class MyGuranteedSingleton : GuranteedAvailabilitySingletonBehaviour<MyGuranteedSingleton>
    {
        public override bool PersistThroughSceneChange => false;
        public override bool HideInInspector => false;
        public override bool HideInHierarchy => false;
    }
}
