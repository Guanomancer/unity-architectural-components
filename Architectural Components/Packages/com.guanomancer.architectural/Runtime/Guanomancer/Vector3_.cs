using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer
{
    public static class Vector3_
    {
        public static Vector3 MultiplyByAxis(Vector3 a, Vector3 b)
            => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);

        public static Vector3 DivideByAxis(Vector3 a, Vector3 b)
            => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);

        public static float Sum(this Vector3 a)
            => a.x + a.y + a.z;

        public static float SqrDistance(this Vector3 a, Vector3 b)
            => Mathf.Pow(a.x - b.x, 2f) + Mathf.Pow(a.y - b.y, 2f) + Mathf.Pow(a.z - b.z, 2f);

        public static Vector3 Abs(this Vector3 a)
            => new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    }
}
