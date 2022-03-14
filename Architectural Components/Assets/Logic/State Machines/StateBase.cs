using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.StateMachines
{
    public abstract class StateBase : MonoBehaviour
    {
        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected virtual void OnUpdate() { }

        public void Enter() => OnEnter();
        public void Exit() => OnExit();
        public void DoUpdate() => OnUpdate();
    }

    public abstract class StateBase<MACHINE_TYPE> : StateBase where MACHINE_TYPE : StateMachine
    {
        public MACHINE_TYPE Context { get; private set; }

        private void Awake() => Context = GetComponent<MACHINE_TYPE>();

        public void Transition(int stateIndex) => Context.Transition(stateIndex);
        public void Transition<STATE_TYPE>() where STATE_TYPE : StateBase => Context.Transition<STATE_TYPE>();
    }
}
