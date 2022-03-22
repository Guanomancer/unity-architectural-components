using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.ProxyGrid
{
    public class ProxyGridController : MonoBehaviour
    {
        [Header("Spawning")]
        [Tooltip("A component that implements IProxyGridSpawner, and can create the grid cell objects.")]
        [SerializeField] private ProxyGridSpawner _spawner;
        [Tooltip("A transformn that holds the spawned grid cell objects. Leave blank to spawn in scene root.")]
        [SerializeField] private Transform _rootObject;
        [Tooltip("The radius inside which objects are spawned.")]
        [SerializeField] private Vector3 _spawnRadius;
        [Tooltip("The radius outside which objects are despawned.")]
        [SerializeField] private Vector3 _despawnRadius;

        [Header("Visibility")]
        [Tooltip("The observer on which's position objects are spawned, shown, hidden and despawned.")]
        [SerializeField] private Transform _observer;
        [Tooltip("The radius inside which objects are shown.")]
        [SerializeField] private Vector3 _showRadius;
        [Tooltip("The radius outside which objects are hidden.")]
        [SerializeField] private Vector3 _hideRadius;

        [Header("Optimization")]
        [Tooltip("Enabling this will spawn objects based on distance from observer, with the closest objects first.")]
        [SerializeField] private bool _nearestFirst;
        [Min(1), Tooltip("Determines how many objects to spawn per Update.")]
        [SerializeField] private int _spawnsPerFrame;

        [Header("Editor")]
        [Tooltip("Hide hidden items in the Unity Editor hierarchy.")]
        [SerializeField] private bool _hideHiddenItemsInHierarchy;

        private Dictionary<Vector3Int, IProxyGridItem> _items = new Dictionary<Vector3Int, IProxyGridItem>();
        private List<IProxyGridItem> _hiddenItems = new List<IProxyGridItem>();
        private List<IProxyGridItem> _shownItems = new List<IProxyGridItem>();
        private List<(Vector3Int, Vector3)> _spawnList = new List<(Vector3Int, Vector3)>();
        private Queue<(Vector3Int gridID, IProxyGridItem gridItem)> _destructionQueue = new Queue<(Vector3Int gridID, IProxyGridItem gridItem)>();
        private Vector3 _centerOffset;
        private Vector3Int _spawnRadiusInt;

        private Transform _hiddenItemParent;

        public int CurrentItems => _items.Count - _spawnList.Count;
        public int SpawnQueueSize => _spawnList.Count;

        private void Start()
        {
            _hiddenItemParent = new GameObject("Hidden").transform;
            _hiddenItemParent.parent = _rootObject;
            if (_hideHiddenItemsInHierarchy)
                _hiddenItemParent.hideFlags = HideFlags.HideInHierarchy;
            _hiddenItemParent.gameObject.SetActive(false);

            _centerOffset = _spawner.TileSize * .5f;
            _spawnRadiusInt = Vector3Int.CeilToInt(Vector3_.DivideByAxis(_spawnRadius, _spawner.TileSize));
        }

        private void Update()
        {
            QueueForSpawning();
            QueueOrAlter();

            SpawnObjects();
            DestroyObjects();
        }

        private void QueueForSpawning()
        {
            var offset = Vector3Int.CeilToInt(Vector3_.DivideByAxis(_observer.position, _spawner.TileSize));
            Vector3Int_.For(-_spawnRadiusInt, _spawnRadiusInt, offset, Vector3Int.one, (gridID) =>
            {
                if (!_items.ContainsKey(gridID))
                {
                    _spawnList.Add((gridID, GetCellCenter(gridID)));
                    _items.Add(gridID, null);
                }
            });
        }

        private void QueueOrAlter()
        {
            using (var enumerator = _items.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var gridID = enumerator.Current.Key;
                    var cell = enumerator.Current.Value;
                    var cellDist = GetDistanceToObserver(gridID);

                    if (cellDist.x >= _despawnRadius.x ||
                        cellDist.y >= _despawnRadius.y ||
                        cellDist.z >= _despawnRadius.z)
                        _destructionQueue.Enqueue((gridID, cell));

                    if (cell == null)
                        continue;

                    if (cellDist.x > _hideRadius.x ||
                        cellDist.y > _hideRadius.y ||
                        cellDist.z > _hideRadius.z)
                        HideCell(cell);

                    if (cellDist.x < _showRadius.x &&
                        cellDist.y < _showRadius.y &&
                        cellDist.z < _showRadius.z)
                        ShowCell(cell);
                }
            }
        }

        private void SpawnObjects()
        {
            Debug.Log($"Spawning {_spawnList.Count} objects.");
            for (int c = 0; c < _spawnsPerFrame; c++)
            {
                if (_spawnList.Count == 0)
                    return;
                int closestSpawnIndex = 0;
                float closestSpawnSqrDistance = _observer.position.SqrDistance(_spawnList[0].Item2);
                for (int i = 1; i < _spawnList.Count; i++)
                {
                    var sqrDistance = _observer.position.SqrDistance(_spawnList[i].Item2);
                    if (sqrDistance < closestSpawnSqrDistance)
                    {
                        closestSpawnIndex = i;
                        closestSpawnSqrDistance = sqrDistance;
                    }
                }

                var cellID = _spawnList[closestSpawnIndex].Item1;
                _spawnList.RemoveAt(closestSpawnIndex);

                var cell = _items[cellID] = _spawner.Spawn(cellID);
                cell.gameObject.transform.position = Vector3_.MultiplyByAxis(cellID, _spawner.TileSize);
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = Vector3.one / 2f;
                cube.transform.parent = cell.gameObject.transform;
                cube.transform.localPosition = Vector3.zero;
                HideCell(cell);
            }
        }

        private void DestroyObjects()
        {
            Debug.Log($"Destroying {_destructionQueue.Count} objects.");
            for (int c = 0; c < _spawnsPerFrame; c++)
            {
                if (_destructionQueue.Count == 0)
                    return;

                var item = _destructionQueue.Dequeue();
                _items.Remove(item.gridID);
                _hiddenItems.Remove(item.gridItem);
                _shownItems.Remove(item.gridItem);
                if (item.gridItem != null)
                    Destroy(item.gridItem.gameObject);
            }
        }

        private void HideCell(IProxyGridItem cell)
        {
            cell.gameObject.transform.parent = _hiddenItemParent;
            _shownItems.Remove(cell);
            _hiddenItems.Add(cell);
            cell.Hide();
        }

        private void ShowCell(IProxyGridItem cell)
        {
            cell.gameObject.transform.parent = _rootObject;
            _hiddenItems.Remove(cell);
            _shownItems.Add(cell);
            cell.Show();
        }

        private Vector3 GetCellCenter(Vector3Int cellID) => Vector3_.MultiplyByAxis(_spawner.TileSize, cellID) + _centerOffset;
        private Vector3 GetDistanceToObserver(Vector3Int cellID) => (GetCellCenter(cellID) - _observer.transform.position).Abs();
    }
}
