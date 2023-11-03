using Cinemachine;
using UnityEngine;

namespace Cinematine
{
    public class Shaking : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachine;
        private CinemachineBasicMultiChannelPerlin _perlin;

        private float timer = 0f;
        private void Start()
        {
            _cinemachine = GetComponent<CinemachineVirtualCamera>();
            _perlin = _cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            StopShake();
        }

        public void Shake(float duration, float intensity)
        {
            _perlin.m_AmplitudeGain = intensity;
            timer = duration;
        }

        private void StopShake()
        {
            _perlin.m_AmplitudeGain = 0f;
            timer = 0f;
        }

        private void Update()
        {
            if (timer < 0) return;
        
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StopShake();
            }
        }
    }
}
