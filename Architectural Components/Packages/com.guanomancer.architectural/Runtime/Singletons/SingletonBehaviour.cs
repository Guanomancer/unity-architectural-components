using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.Singletons
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        public abstract bool PersistThroughSceneChange { get; }
        public abstract bool HideInInspector { get; }
        public abstract bool HideInHierarchy { get; }

        public virtual void Awake()
        {
            if(Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this as T;

            if(PersistThroughSceneChange)
                DontDestroyOnLoad(this);
            
            hideFlags = HideFlags.DontSave;
            if (HideInInspector)
                hideFlags |= HideFlags.HideInInspector;
            if (HideInHierarchy)
                hideFlags |= HideFlags.HideInHierarchy;
        }
    }
}
