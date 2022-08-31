using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraMovements : MonoBehaviour
{
    [SerializeField] private bool freezeX;
    [SerializeField] private bool freezeY;
    [SerializeField] private bool freezeZ;

    [SerializeField] private bool isFollowing;
    [SerializeField] private float smoothing;
    [SerializeField] private float rotationLimit;
    [SerializeField] private float halfWidth;
    [SerializeField] private float repositionLimitX;

    private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
        GameManager.instance.OnLifeLost += () => StartCoroutine(Shake());
    }

    public float smoothSpeed = 0.125f;

    private float desiredX, desiredY, desiredZ;
    private Vector3 desiredPosition, smoothedPosition, currentPosition;

    private void FixedUpdate()
    {
        if (!isFollowing) return;

        currentPosition = transform.position;

        desiredX = freezeX
            ? currentPosition.x
            : Mathf.Clamp(target.position.x + offset.x, -repositionLimitX, repositionLimitX);

        desiredY = freezeY ? currentPosition.y : target.position.y + offset.y;
        desiredZ = freezeZ ? currentPosition.z : target.position.z + offset.z;
        desiredPosition = new Vector3(desiredX, desiredY, desiredZ);

        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }


    private float targetRotationZ, currentRotationZ, deltaRotation, dif, projectedRotationAmount;

    public void Rotate(float playerPositionX, float centerX)
    {
        targetRotationZ = (playerPositionX - centerX) / halfWidth * rotationLimit;
        currentRotationZ = transform.eulerAngles.z;
        projectedRotationAmount = targetRotationZ - currentRotationZ;
        deltaRotation = Mathf.Clamp(projectedRotationAmount, -rotationLimit, rotationLimit);
        transform.Rotate(Vector3.forward, projectedRotationAmount);
    }

    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float shakeDuration = 0.5f;


    public IEnumerator Shake()
    {
        var t = shakeDuration;
        while (t > 0)
        {
            t -= Time.deltaTime;
            transform.localPosition += Random.insideUnitSphere * shakeAmount;
            // t -= Time.deltaTime * decreaseFactor;
            yield return null;
        }
    }
}