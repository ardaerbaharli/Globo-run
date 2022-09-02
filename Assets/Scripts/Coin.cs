using Managers;
using UnityEngine;

public class Coin : Collectable
{
    [SerializeField] private int amount;

    private bool _didCollect;

    protected override void Collect()
    {
        _didCollect = true;
        CollectableSpawner.Instance.Spawn();
        print("Activate");
        ScoreManager.instance.Coin += amount;
    }

    public override void Missed()
    {
        if (_didCollect) return;
        print("missed");
        CollectableSpawner.Instance.Spawn();
        ObjectPool.Instance.TakeBack(pooledObject);
    }
}