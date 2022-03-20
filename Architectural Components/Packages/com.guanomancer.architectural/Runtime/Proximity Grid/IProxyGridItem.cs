using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.ProxyGrid
{
    public interface IProxyGridItem
    {
        public void Show();
        public void Hide();

        public GameObject gameObject { get; }
    }
}
