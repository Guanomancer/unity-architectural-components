using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.EventRouting
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Guanomancer/Game Event")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField] private static bool _logEvents;
        [SerializeField] private static string _logFormat = "<color=#7777cc>Game event {0} dispatched by {1}</color>\n{2}";

        private List<(IGameEventSubscriber subscriber, GameEventHandler handler)> _handlers = new List<(IGameEventSubscriber subscriber, GameEventHandler handler)>();

        public void Subscribe(IGameEventSubscriber subscriber, GameEventHandler handler)
            => _handlers.Add((subscriber, handler));

        public void Unsubscribe(IGameEventSubscriber subscriber)
        {
            for (int i = 0; i < _handlers.Count; i++)
            {
                if (_handlers[i].subscriber == subscriber)
                {
                    _handlers.RemoveAt(i);
                    return;
                }
            }
        }

        public bool IsSubscribed(IGameEventSubscriber subscriber)
        {
            for (int i = 0; i < _handlers.Count; i++)
                if (_handlers[i].subscriber == subscriber)
                    return true;
            return false;
        }

        public void Dispatch(object sender, object context)
        {
            Debug.Log(string.Format(_logFormat, name, sender.GetType().Name,
                context == null ? "" : context.GetType().IsPrimitive ? context : JsonUtility.ToJson(context, true)));

            for (int i = 0; i < _handlers.Count; i++)
                _handlers[i].handler(sender, context);
        }
    }
}
