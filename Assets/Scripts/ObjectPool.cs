using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

[Serializable]
public class ObjectToPool
{
    public string name;
    public bool autoName = true;
    public GameObject gameObject;
    public int amount;
    public bool isObstacle;
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
    public static ObjectPool instance;
    public List<ObjectToPool> objectToPool;
    public Queue<PooledObject> pooledObjectsQ;
    public Dictionary<string, Queue<PooledObject>> poolDictionary;

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
        instance = this;
        StartPool();
    }

    private void OnGameOver()
    {
        // set active false every child of parent
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void StartPool()
    {
        if (parent == null) parent = transform;

        poolDictionary = new Dictionary<string, Queue<PooledObject>>();
        foreach (var item in objectToPool)
        {
            pooledObjectsQ = new Queue<PooledObject>();
            for (var i = 0; i < item.amount; i++)
            {
                var obj = Instantiate(item.gameObject, parent.transform);

                obj.SetActive(false);

                pooledObjectsQ.Enqueue(new PooledObject()
                {
                    name = item.name,
                    gameObject = obj,
                    transform = obj.transform,
                    poolName = item.name
                });
            }

            poolDictionary.Add(item.name, pooledObjectsQ);
        }

        isPoolSet = true;
    }


    public PooledObject GetPooledObject(string objectName, bool setActive = false)
    {
        if (!poolDictionary.ContainsKey(objectName))
        {
            return null;
        }

        var obj = poolDictionary[objectName].Dequeue();
        if (obj.gameObject.activeSelf)
            return GetPooledObject(objectName);

        var prefabRotation = objectToPool.First(x => x.name == objectName).gameObject.transform.rotation;
        obj.transform.rotation = prefabRotation;

        obj.gameObject.SetActive(setActive);
        return obj;
    }

    public PooledObject GetObstaclePooledObject()
    {
        objectToPool.Shuffle();
        var objName = objectToPool.First(x => x.isObstacle).name;
        return GetPooledObject(objName);
    }

    public void TakeBack(PooledObject obj)
    {
        if (!gameObject.activeSelf) return;
        if (obj.gameObject == null) return;

        obj.gameObject.SetActive(false);
        var objectName = obj.name;
        poolDictionary[objectName].Enqueue(obj);
    }
}