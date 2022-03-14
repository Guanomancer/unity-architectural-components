using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.StateMachines
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private string _stateName;

        [Header("States")]
        [SerializeField]
        private bool _resetOnEnable = false;
        [SerializeField]
        private StateBase _initialState;

        private StateBase[] _states;
        private StateBase _currentState;

        public void Transition(StateBase state)
        {
            if (_currentState == state)
                return;
            _currentState?.Exit();
            _currentState = state;
            _stateName = _currentState == null ? "" : _currentState.GetType().Name;
            _currentState?.Enter();
        }

        public void Transition(int stateIndex)
            => Transition(stateIndex < 0 || stateIndex > _states.Length ? null : _states[stateIndex]);

        public void Transition<STATE_TYPE>() where STATE_TYPE : StateBase
        {
            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i].GetType() == typeof(STATE_TYPE))
                {
                    Transition(i);
                    return;
                }
            }
            throw new UnityException($"State {typeof(STATE_TYPE).Name} not found.");
        }

        private void Start()
        {
            _states = GetComponents<StateBase>();

            if (_initialState != null)
                Transition(_initialState);
            else
                Transition(0);
        }

        private void OnEnable()
        {
            if (_resetOnEnable)
                Transition(-1);
            else
                _currentState?.Exit();
        }

        private void OnDisable()
        {
            if (_resetOnEnable)
                Transition(0);
            else
                _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.DoUpdate();
        }
    }
}
