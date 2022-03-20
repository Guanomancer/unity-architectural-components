using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.StateMachines.TestsPlayMode
{
    public class EndState : StateBase<StateMachine>
    {
        protected override void OnEnter()
        {
            Debug.Log("Enter End");
        }

        protected override void OnExit()
        {
            Debug.Log("Exit End");
        }

        protected override void OnUpdate()
        {
            Debug.Log("Update End");
        }
    }
}
