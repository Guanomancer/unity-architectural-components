using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.StateMachines
{
    public class StartState : StateBase<StateMachine>
    {
        protected override void OnEnter()
        {
            Debug.Log("Enter Start");
        }

        protected override void OnExit()
        {
            Debug.Log("Exit Start");
        }

        protected override void OnUpdate()
        {
            Transition<EndState>();
        }
    }
}
