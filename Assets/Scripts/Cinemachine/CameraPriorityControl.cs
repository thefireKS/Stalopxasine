using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
    public class CameraPriorityControl : MonoBehaviour
    {
        [Serializable]
        public struct CameraDictionary
        {
            public string cameraName;
            public CinemachineVirtualCamera camera;
        }

        [SerializeField] private CameraDictionary[] cameraDictionary;
        
        private Dictionary<string, CinemachineVirtualCamera> _cameras = new();
        private List<CinemachineVirtualCamera> _cameraList = new();

        [SerializeField] private int elevatedPriority = 15, loweredPriority = 10;

        private void Start()
        {
            foreach (var zapis in cameraDictionary)
            {
                _cameras.Add(zapis.cameraName, zapis.camera);
            }
            
            _cameraList.AddRange(_cameras.Values);
            
            SetCamerasPriority("Gameplay");
        }

        public CinemachineVirtualCamera GetCameraByName(string cameraName)
        {
            if (_cameras.TryGetValue(cameraName, out var getCameraByName))
            {
                return getCameraByName;
            }
            
            Debug.LogError($"Camera with name {cameraName} not found!");
            return null;
        }

        public void SetCamerasPriority(string targetCameraName)
        {
            foreach (var cameraPair in _cameras)
            {
                var cameraPairValue = cameraPair.Value;
                if (cameraPair.Key == targetCameraName)
                {
                    // Устанавливаем повышенный приоритет для целевой камеры
                    cameraPairValue.Priority = elevatedPriority;
                }
                else
                {
                    // Устанавливаем пониженный приоритет для остальных камер
                    cameraPairValue.Priority = loweredPriority;
                }
            }
        }
    }
}