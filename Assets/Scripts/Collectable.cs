using System;
using Managers;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] private float height;

    public PooledObject pooledObject;
    private Renderer _renderer;
    private Light _light;

    private void OnEnable()
    {
        _light = GetComponentInParent<Light>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnDisable()
    {
        _renderer.enabled = true;
        _light.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _renderer.enabled = false;
            _light.enabled = false;
            Collect();
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.parent.position = position + Vector3.up * height;
    }

    protected abstract void Collect();
    public abstract void Missed();
}