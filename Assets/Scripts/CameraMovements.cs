using System;
using System.Collections;
using Cinemachine;
using Enums;
using Managers;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    [SerializeField] private float rotationLimit;
    [SerializeField] private float halfWidth;
    [SerializeField] private CinemachineVirtualCamera cmvm;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float amplitudeGain;
    [SerializeField] private float frequencyGain;

    [SerializeField] private float NORMAL_FOV;
    [SerializeField] private float minFov;
    [SerializeField] private float maxFov;
    
    private float _targetRotationZ;
    private Player _player;
    public static CameraMovements Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnLifeLost += () => StartCoroutine(Shake());
    }


    public void ChangeFOV(float fov)
    {
        fov *= NORMAL_FOV;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        cmvm.m_Lens.FieldOfView = fov;
    }

    public void Rotate(float playerPositionX, float centerX)
    {
        _targetRotationZ = (playerPositionX - centerX) / halfWidth * rotationLimit;
        cmvm.m_Lens.Dutch = _targetRotationZ;
    }


    private IEnumerator Shake()
    {
        var noise = cmvm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
        yield return new WaitForSeconds(shakeDuration);

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}