using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Guanomancer.EventRouting
{
    public class EventSubscriber : MonoBehaviour, IEventSubscriber
    {
        [SerializeField]
        public List<EventBinding> Bindings;

        public void OnEnable()
        {
            if (Bindings == null)
                return;
            
            foreach(var binding in Bindings)
                EventRouter.Subscribe(binding.EventName, this);
        }
        
        public void OnDisable()
        {
            if (Bindings == null)
                return;

            foreach (var binding in Bindings)
                EventRouter.Unsubscribe(binding.EventName, this);
        }

        public void OnEvent(IEventContext context)
        {
            var name = context.GetType().Name;
            foreach (var binding in Bindings)
            {
                if (binding.EventName == name)
                {
                    binding.OnEvent.Invoke(context);
                    return;
                }
            }
        }
    }

    [System.Serializable]
    public class EventBinding
    {
        public string EventName;
        public UnityEvent<IEventContext> OnEvent;
    }
}
