using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    [SerializeField] private float rotationLimit;
    [SerializeField] private float halfWidth;
    [SerializeField] private CinemachineVirtualCamera cmvm;
    [SerializeField] private float shakeDuration = 0.5f;

    private float _targetRotationZ;
    private Player _player;


    private void Start()
    {
        GameManager.Instance.OnLifeLost += () => StartCoroutine(Shake());
    }


    public void Rotate(float playerPositionX, float centerX)
    {
        _targetRotationZ = (playerPositionX - centerX) / halfWidth * rotationLimit;
        cmvm.m_Lens.Dutch = _targetRotationZ;
    }


    private IEnumerator Shake()
    {
        var noise = cmvm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0.7f;
        noise.m_FrequencyGain = 1f;
        yield return new WaitForSeconds(shakeDuration);

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}