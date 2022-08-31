using System;
using System.Collections;
using Cinemachine;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraMovements : MonoBehaviour
{
    [SerializeField] private float rotationLimit;
    [SerializeField] private float halfWidth;
    [SerializeField] private CinemachineVirtualCamera cmvm;
    [SerializeField] private float shakeDuration = 0.5f;

    private Player player;


    private void Start()
    {
        GameManager.instance.OnLifeLost += () => StartCoroutine(Shake());
    }

    private float targetRotationZ;


    public void Rotate(float playerPositionX, float centerX)
    {
        targetRotationZ = (playerPositionX - centerX) / halfWidth * rotationLimit;
        cmvm.m_Lens.Dutch = targetRotationZ;
    }


    private IEnumerator Shake()
    {
        var noise = cmvm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0.5f;
        noise.m_FrequencyGain = 1f;
        yield return new WaitForSeconds(shakeDuration);

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}