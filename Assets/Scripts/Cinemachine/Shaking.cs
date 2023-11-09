using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Cinematine
{
    public class Shaking : MonoBehaviour
    {
        private List<CinemachineVirtualCamera> _cinemachines = new List<CinemachineVirtualCamera>();
        private CinemachineBasicMultiChannelPerlin _perlin;

        private float timer = 0f;
        private void Start()
        {
            _cinemachines.AddRange(FindObjectsOfType<CinemachineVirtualCamera>());
            GetActiveVirtualCamera();
            StopShake();
        }

        private void GetActiveVirtualCamera()
        {
            foreach (var cinemachine in _cinemachines)
            {
                if (CinemachineCore.Instance.IsLive(cinemachine))
                    _perlin = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }

        public void Shake(float duration, float intensity)
        {
            GetActiveVirtualCamera();
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
