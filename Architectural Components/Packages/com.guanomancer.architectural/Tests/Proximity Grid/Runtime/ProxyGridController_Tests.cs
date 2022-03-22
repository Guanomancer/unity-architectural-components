using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Guanomancer.ProxyGrid.TestsPlayMode
{
    public class ProxyGridController_Tests
    {
        private ProxyGridController _grid;
        private TestSpawner _spawner;
        private GameObject _observer;

        public void Setup(int spawnsPerFrame = 1)
        {
            _spawner = new GameObject("Spawner").AddComponent<TestSpawner>();

            _observer = new GameObject("Observer");

            var obj = new GameObject("Grid");
            _grid = obj.AddComponent<ProxyGridController>();
            _grid.SetPrivateFieldsInTest(
                new (string, object)[] {
                    ("_spawner", _spawner),
                    ("_rootObject", obj.transform),
                    ("_spawnRadius", _spawner.TileSize * 2),
                    ("_despawnRadius", _spawner.TileSize * 4),
                    ("_observer", _observer.transform),
                    ("_showRadius", _spawner.TileSize * 1),
                    ("_hideRadius", _spawner.TileSize * 3),
                    ("_nearestFirst", true),
                    ("_spawnsPerFrame", spawnsPerFrame),
                    ("_hideHiddenItemsInHierarchy", false),
                    }
                );
        }

        [UnityTest]
        public IEnumerator EventController_SpawnsCorrectAmountOfCellsPerFrame()
        {
            Setup();

            yield return null;
            Assert.AreEqual(1, _grid.CurrentItems);

            yield return null;
            Assert.AreEqual(2, _grid.CurrentItems);
        }

        [UnityTest]
        public IEnumerator EventController_SpawnNewCellsWhenMovingObserver()
        {
            Setup(10000);
            _grid.SetPrivateFieldInTest("_despawnRadius", _spawner.TileSize * 1000);

            yield return null;
            Assert.AreEqual(125, _grid.CurrentItems);

            for (int i = 0; i < 10; i++)
            {
                _observer.transform.position += Vector3.forward * _spawner.TileSize.z;
                yield return null;
                Assert.AreEqual(150 + i * 25, _grid.CurrentItems);
            }
        }

        [UnityTest]
        public IEnumerator EventController_DespawnsWhenFar()
        {
            Setup(10000);

            yield return null;

            for (int i = 0; i < 10; i++)
            {
                _observer.transform.position += Vector3.forward * _spawner.TileSize.z;
                yield return null;
                Assert.IsTrue(175 >= _grid.CurrentItems);
            }
        }
    }

    [SerializeField]
    public class TestSpawner : ProxyGridSpawner
    {
        public override Vector3 TileSize => Vector3.one;

        public override IProxyGridItem Spawn(Vector3Int gridID)
        {
            var obj = new GameObject($"Cell {gridID}");
            var item = obj.AddComponent<TestItem>();
            return item;
        }
    }

    public class TestItem : MonoBehaviour, IProxyGridItem
    {
        public void Hide()
        {
            enabled = false;
        }

        public void Show()
        {
            enabled = true;
        }
    }
}
