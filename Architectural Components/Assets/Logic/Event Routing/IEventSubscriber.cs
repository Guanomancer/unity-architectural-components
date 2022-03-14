using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.EventRouting
{
    public interface IEventSubscriber
    {
        public void OnEvent(IEventContext context);
    }
}
