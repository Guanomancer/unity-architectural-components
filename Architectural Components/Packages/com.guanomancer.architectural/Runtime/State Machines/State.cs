using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.StateMachines
{
    public class StateBehaviour : MonoBehaviour
    {
        public void Transition<T>() where T : StateBehaviour
        {
            enabled = false;
            var state = GetComponent<T>();
            if(state != null)
                state.enabled = true;
        }
    }
}
