using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.ProxyGrid
{
    public interface IProxyGridSpawner
    {
        public Vector3 TileSize { get; }
        public IProxyGridItem Spawn(Vector3Int gridID);
    }
}
