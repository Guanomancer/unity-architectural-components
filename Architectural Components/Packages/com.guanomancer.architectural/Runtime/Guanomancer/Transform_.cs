using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer
{
    public static class Transform_
    {
        public static void DestroyChildren(this Transform transform)
        {
            var children = new Transform[transform.childCount];
            for (int i = 0; i < children.Length; i++)
                children[i] = transform.GetChild(i);

            if (Application.isPlaying)
                for (int i = 0; i < children.Length; i++)
                    GameObject.Destroy(children[i].gameObject);
            else
                for (int i = 0; i < children.Length; i++)
                    GameObject.DestroyImmediate(children[i].gameObject);
        }
    }
}
