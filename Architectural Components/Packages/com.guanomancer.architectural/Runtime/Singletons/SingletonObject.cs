using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.Singletons
{
    public abstract class SingletonObject<T> : ScriptableObject where T : SingletonObject<T>
    {
        private static string _path = "";

        public static void SetPath(string path) => _path = path;

        private static SingletonObject<T> _instance;
        
        public static SingletonObject<T> Instance
        {
            get
            {
                if(_instance == null)
                {
                    var objects = Resources.LoadAll<T>(_path);
                    if (objects.Length == 0)
                        throw new UnityException($"No {typeof(T).Name} found in the Resources folder.");
                    else if (objects.Length > 1)
                        throw new UnityException($"More than a single {typeof(T).Name} found in the Resources folder.");
                    else
                        _instance = objects[0];
                }
                return _instance;
            }
        }
    }
}
