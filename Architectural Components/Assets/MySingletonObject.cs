using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guanomancer.Singletons;

namespace Guanomancer
{
    [CreateAssetMenu(fileName = "New Test Singleton Object", menuName = "Test/Singleton Object")]
    public class MySingletonObject : SingletonObject<MySingletonObject>
    {

    }
}
