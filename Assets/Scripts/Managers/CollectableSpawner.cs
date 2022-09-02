using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private float spawnEveryZ;
        [SerializeField] private float leftBorderX;
        [SerializeField] private float rightBorderX;

        private Vector3 _needDif;
        private int _planeLength;
        public static CollectableSpawner Instance;
        private Queue<Collectable> _spawnedCollectables;
        private Vector3 _lastSpawnedPosition;
        [SerializeField] private LayerMask layerMask;


        private void Awake()
        {
            Instance = this;
            _lastSpawnedPosition = startPosition;
            _spawnedCollectables = new Queue<Collectable>();
            _needDif = Vector3.zero;
            layerMask = ~layerMask;
        }

        public void Spawn()
        {
            var collectablePooledObject = ObjectPool.Instance.GetCollectablePooledObject();
            collectablePooledObject.gameObject.SetActive(true);

            var collectable = collectablePooledObject.gameObject.GetComponentInChildren<Collectable>();
            collectable.pooledObject = collectablePooledObject;
            _spawnedCollectables.Enqueue(collectable);

            var pos = GetEmptyPos();
            collectable.SetPosition(pos);
        }

        private Vector3 GetEmptyPos()
        {
            // check if is there any obstacle in the way
            var b = true;
            while (b)
            {
                var pos = _lastSpawnedPosition + Vector3.forward * spawnEveryZ + _needDif;
                pos.x = Random.Range(leftBorderX, rightBorderX);
                var rayPos = pos + Vector3.up * 10f;
                var ray = new Ray(rayPos, Vector3.down);
                var sphere = new Vector3(1, 1, 1);
                Debug.DrawRay(rayPos, Vector3.down * 15f, Color.white, 3);

                if (Physics.CheckSphere(rayPos, 18f, layerMask))
                {
                    print("Somethings in the way. Trying again...");
                    _needDif += Vector3.forward;
                }
                else
                {
                    _needDif = Vector3.zero;
                    _lastSpawnedPosition = pos;
                    Debug.DrawRay(rayPos, Vector3.down * 30f, Color.green, 5);
                    return pos;
                }
            }

            return Vector3.zero;
        }


        public void Init()
        {
            Spawn();
        }
    }
}