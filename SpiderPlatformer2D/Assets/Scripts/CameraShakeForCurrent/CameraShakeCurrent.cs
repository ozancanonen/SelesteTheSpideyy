using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeCurrent : MonoBehaviour
{

    //We need to add noise to our camera with 6D Shake.
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    void OnEnable()
    {
        // Get Virtual Camera Noise Profile
        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        GameManager.shakeCurrentCam += ShakeCamera;
    }

    private void ShakeCamera()
    {
        StartCoroutine(ShakeCameraWithDelay(ShakeDuration));
    }

    private IEnumerator ShakeCameraWithDelay(float time)
    {
        virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = ShakeFrequency;
        yield return new WaitForSeconds(time);
        virtualCameraNoise.m_AmplitudeGain = 0;
        virtualCameraNoise.m_FrequencyGain = 0;
    }

    private void OnDisable()
    {
        GameManager.shakeCurrentCam -= ShakeCamera;
    }










}
