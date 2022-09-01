using System;
using System.Collections.Generic;
using Enums;
using Extensions;
using NaughtyAttributes;
using PowerUps;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.WSA;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float spawnEveryZ;
    [EnumFlags] [SerializeField] private PowerUpType selectedPowerUpTypes;

    private Vector3 _needDif;
    private int _index;
    private int _planeLength;
    private LevelManager _levelManager;
    private IEnumerable<Enum> _uniqueFlags;
    public static PowerUpSpawner Instance;
    private Queue<PowerUp> _spawnedPowerUps;
    private Vector3 _lastSpawnedPosition;


    private void Awake()
    {
        Instance = this;
        _lastSpawnedPosition = startPosition;
        _spawnedPowerUps = new Queue<PowerUp>();
        _levelManager = GetComponent<LevelManager>();
        _needDif = Vector3.zero;
        _uniqueFlags = selectedPowerUpTypes.GetUniqueFlags();
    }

    public void Spawn()
    {
        var randomPowerUpType = (PowerUpType) _uniqueFlags.RandomElement();
        var powerUpPooledObject = ObjectPool.Instance.GetPooledObject("PowerUp");
        powerUpPooledObject.gameObject.SetActive(true);

        var powerUp = powerUpPooledObject.gameObject.GetComponent<PowerUp>();
        powerUp.pooledObject = powerUpPooledObject;
        powerUp.SetPowerUpType(randomPowerUpType);
        _spawnedPowerUps.Enqueue(powerUp);

        var pos = GetEmptyPos();
        print(pos);
        powerUp.transform.position = pos;
        _index++;
    }

    private Vector3 GetEmptyPos()
    {
        // check if is there any obstacle in the way
        var b = true;
        while (b)
        {
            var pos = _lastSpawnedPosition + Vector3.forward * spawnEveryZ + _needDif; // * _index
            var rayPos = pos + Vector3.up * 10f;
            var ray = new Ray(rayPos, Vector3.down);
            Debug.DrawRay(rayPos, Vector3.down * 15f, Color.white, 3);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject.CompareTag("Plane"))
                {
                    _needDif = Vector3.zero;
                    _lastSpawnedPosition = pos;
                    return pos;
                }

                print("Somethings in the way. Trying again...");
            }

            _needDif += Vector3.forward;
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        var lastDrewPosition = startPosition;
        Gizmos.color = Color.red;
        for (int i = 0; i < 50; i++)
        {
            var p = lastDrewPosition + Vector3.forward * spawnEveryZ + _needDif;
            Gizmos.DrawWireSphere(p, 5f);
            lastDrewPosition = p;
        }
    }

    public void Init()
    {
        Spawn();
    }
}