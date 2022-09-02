using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public PooledObject pooledObject;
    // [SerializeField] private float boxLengthZ;
    // [SerializeField] private float leftPos;
    // [SerializeField] private float rightPos;
    //
    // private List<GameObject> _boxes;
    //
    //
    // public void SetSize(int lengthZ)
    // {
    //     _boxes = new List<GameObject>();
    //     var count = lengthZ / boxLengthZ;
    //     for (var j = 0; j < 2; j++)
    //     {
    //         var wallParent = new GameObject();
    //         wallParent.transform.SetParent(transform);
    //         wallParent.name = j == 1 ? "LeftWall" : "RightWall";
    //
    //         for (var i = 0; i < 10; i++)
    //         {
    //             var wall = ObjectPool.Instance.GetRandomWall();
    //             wall.transform.SetParent(transform);
    //             wall.transform.localPosition = new Vector3(j == 1 ? leftPos : rightPos, 0, i * boxLengthZ);
    //             wall.gameObject.SetActive(true);
    //             _boxes.Add(wall.gameObject);
    //         }
    //     }
    // }

    public void TakeBack()
    {
        ObjectPool.Instance.TakeBack(pooledObject);
    }
}