using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.StateMachines
{
    public class StateTestA : StateBehaviour
    {
        private void OnEnable()
            => Debug.Log($"Enable {GetType().Name} on {transform.name}.");
        private void OnDisable()
            => Debug.Log($"Disable {GetType().Name} on {transform.name}.");
        private void Update()
            => Transition<StateTestB>();
    }
}
