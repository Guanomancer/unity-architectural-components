using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.ProxyGrid
{
    public abstract class ProxyGridSpawner : MonoBehaviour
    {
        public abstract Vector3 TileSize { get; }
        public abstract IProxyGridItem Spawn(Vector3Int gridID);
    }
}
