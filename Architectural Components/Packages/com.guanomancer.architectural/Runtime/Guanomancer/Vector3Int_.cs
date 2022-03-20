using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer
{
    public static class Vector3Int_
    {
        public static int Product(this Vector3Int vector)
            => vector.x * vector.y * vector.z;

        public static void For(Vector3Int start, Vector3Int end, Vector3Int step, System.Action<Vector3Int> action)
        {
            var count = Vector3Int.zero;
            for (count.x = start.x; count.x <= end.x; count.x += step.x)
                for (count.y = start.y; count.y <= end.y; count.y += step.y)
                    for (count.z = start.z; count.z <= end.z; count.z += step.z)
                        action(count);
        }

        public static void For<T_STATE>(Vector3Int start, Vector3Int end, Vector3Int step, System.Action<Vector3Int, T_STATE> action, T_STATE state)
        {
            var count = Vector3Int.zero;
            for (count.x = start.x; count.x <= end.x; count.x += step.x)
                for (count.y = start.y; count.y <= end.y; count.y += step.y)
                    for (count.z = start.z; count.z <= end.z; count.z += step.z)
                        action(count, state);
        }

        public static void For(Vector3Int start, Vector3Int end, Vector3Int offset, Vector3Int step, System.Action<Vector3Int> action)
            => For(start + offset, end + offset, step, action);

        public static void For<T_STATE>(Vector3Int start, Vector3Int end, Vector3Int offset, Vector3Int step, System.Action<Vector3Int, T_STATE> action, T_STATE state)
            => For<T_STATE>(start + offset, end + offset, step, action, state);
    }
}
