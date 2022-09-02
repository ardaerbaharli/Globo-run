using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Managers
{
    [Serializable]
    public class ObjectToPool
    {
        public string name;
        public bool autoName = true;
        public GameObject gameObject;
        public int amount;
        public bool isObstacle;
        public bool isCollectable;
    }

    [Serializable]
    public class PooledObject
    {
        public string name;
        public GameObject gameObject;
        public Transform transform;
        public string poolName;
    }

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;
        public List<ObjectToPool> objectToPool;
        public Queue<PooledObject> PooledObjectsQ;
        public Dictionary<string, Queue<PooledObject>> PoolDictionary;

        public bool isPoolSet;
        public Transform parent;

        private void OnValidate()
        {
            foreach (var op in objectToPool)
            {
                if (op.autoName)
                    op.name = op.gameObject.name;
                if (op.name.Contains("Obstacle")) op.isObstacle = true;
            }
        }

        private void Awake()
        {
            Instance = this;
            StartPool();
        }

        private void StartPool()
        {
            if (parent == null) parent = transform;

            PoolDictionary = new Dictionary<string, Queue<PooledObject>>();
            foreach (var item in objectToPool)
            {
                PooledObjectsQ = new Queue<PooledObject>();
                for (var i = 0; i < item.amount; i++)
                {
                    var obj = Instantiate(item.gameObject, parent.transform);

                    obj.SetActive(false);

                    PooledObjectsQ.Enqueue(new PooledObject()
                    {
                        name = item.name,
                        gameObject = obj,
                        transform = obj.transform,
                        poolName = item.name
                    });
                }

                PoolDictionary.Add(item.name, PooledObjectsQ);
            }

            isPoolSet = true;
        }


        public PooledObject GetPooledObject(string objectName, bool setActive = false)
        {
            if (!PoolDictionary.ContainsKey(objectName))
            {
                return null;
            }

            var obj = PoolDictionary[objectName].Dequeue();
            if (obj.gameObject.activeSelf)
                return GetPooledObject(objectName);

            var prefabRotation = objectToPool.First(x => x.name == objectName).gameObject.transform.rotation;
            obj.transform.rotation = prefabRotation;
            obj.transform.localScale = objectToPool.First(x => x.name == objectName).gameObject.transform.localScale;
            obj.gameObject.SetActive(setActive);
            return obj;
        }

        public PooledObject GetObstaclePooledObject()
        {
            objectToPool.Shuffle();
            var objName = objectToPool.First(x => x.isObstacle).name;
            return GetPooledObject(objName);
        }

        public PooledObject GetCollectablePooledObject()
        {
            objectToPool.Shuffle();
            var objName = objectToPool.First(x => x.isCollectable).name;
            return GetPooledObject(objName);
        }

        public void TakeBack(PooledObject obj)
        {
            if (!gameObject.activeSelf) return;
            if (obj.gameObject == null) return;

            obj.gameObject.SetActive(false);
            var objectName = obj.name;
            PoolDictionary[objectName].Enqueue(obj);
        }
    }
}