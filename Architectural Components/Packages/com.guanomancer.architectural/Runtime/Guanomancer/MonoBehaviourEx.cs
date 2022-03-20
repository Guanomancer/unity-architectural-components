using System.Reflection;
using UnityEngine;

namespace Guanomancer
{
    public static class MonoBehaviourEx
    {
        public static void SetPrivateFieldInTest(this MonoBehaviour behaviour, string name, object value)
        {
            var field = behaviour.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
                field = behaviour.GetType().GetField(name);
            if (field == null)
                throw new UnityException($"No field {name} found in {behaviour.GetType().Name}.");
            if (!field.FieldType.IsAssignableFrom(value.GetType()))
                throw new UnityException($"Field {name} takes a {field.FieldType.Name} and not the provided {value.GetType()}.");
            field.SetValue(behaviour, value);
        }

        public static void SetPrivateFieldsInTest(this MonoBehaviour behaviour, (string name, object value)[] fields)
        {
            foreach ((string name, object value) in fields)
                SetPrivateFieldInTest(behaviour, name, value);
        }
    }
}
