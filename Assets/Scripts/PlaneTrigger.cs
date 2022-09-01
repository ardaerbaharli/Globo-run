using System;
using UnityEngine;

public class PlaneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetComponentInParent<Plane>().TakeBack();
    }
}