using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.Singletons
{
    public abstract class GuranteedAvailabilitySingletonBehaviour : MonoBehaviour
    {
        public abstract bool PersistThroughSceneChange { get; }
        public abstract bool HideInInspector { get; }
        public abstract bool HideInHierarchy { get; }
    }

    public abstract class GuranteedAvailabilitySingletonBehaviour<T> : GuranteedAvailabilitySingletonBehaviour where T : GuranteedAvailabilitySingletonBehaviour
    {
        private static T _instance;
        private static bool _isInitializing;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    var obj = new GameObject(typeof(T).Name);
                    _isInitializing = true;
                    _instance = obj.AddComponent<T>();
                    _isInitializing = false;

                    if (_instance.PersistThroughSceneChange)
                        DontDestroyOnLoad(_instance);

                    _instance.hideFlags = HideFlags.DontSave;
                    if (_instance.HideInInspector)
                        _instance.hideFlags |= HideFlags.HideInInspector;
                    if (_instance.HideInHierarchy)
                        _instance.hideFlags |= HideFlags.HideInHierarchy;
                }

                return _instance;
            }
        }

        public virtual void OnEnable()
        {
            if(_instance != this && !_isInitializing)
                Debug.LogError($"A {nameof(GuranteedAvailabilitySingletonBehaviour)} should only be created through the {nameof(Instance)} property.", this);
        }
    }
}
