using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guanomancer.Singletons;

namespace Guanomancer
{
    public class SingletonObject_TestBehaviour : MonoBehaviour
    {
        [ContextMenu("Default")]
        public void Test_Default()
        {
            MySingletonObject.SetPath("");
            Debug.Log("Instance: " + MySingletonObject.Instance);
        }
    }
}
