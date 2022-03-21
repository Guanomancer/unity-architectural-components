using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Guanomancer.EventRouting
{
    public class GameEventController : MonoBehaviour, IGameEventSubscriber
    {
        [SerializeField]
        private GameEventControllerEntry[] _events;

        public void OnEnable()
        {
            if (_events == null)
                return;

            for (int i = 0; i < _events.Length; i++)
            {
                var e = _events[i];
                _events[i].GameEvent.Subscribe(this, (sender, context) => { e.UnityEvent.Invoke(sender, context); });
            }
        }

        public void OnDisable()
        {
            for (int i = 0; i < _events.Length; i++)
                _events[i].GameEvent.Unsubscribe(this);
        }
    }

    [System.Serializable]
    public struct GameEventControllerEntry
    {
        public GameEvent GameEvent;
        public UnityEvent<object, object> UnityEvent;
    }
}
