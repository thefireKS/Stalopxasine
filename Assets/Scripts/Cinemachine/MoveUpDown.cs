using System;
using Player;
using UnityEngine;

namespace Cinemachine
{
    public class MoveUpDown : MonoBehaviour
    {
        public float speed = 2.0f;
        
        private float _speedFactor;
        [SerializeField] private float backSpeedFactor = 3f;
        
        public float cameraAdjustment = 10.0f;

        private CinemachineFramingTransposer _cinemachineFramingTransposer;

        private Controller _controller;

        private float _defaultOffset;
        private float _targetOffset;

        private float _minOffset, _maxOffset;

        private int _lastDirection;
        private int _currentDirection;


        [SerializeField] private float timeToStartMove;
        private Timer _timer;

        private void Start()
        {
            _controller = FindObjectOfType<Controller>();

            _timer = gameObject.AddComponent<Timer>();
            _timer.SetTime(timeToStartMove);
            
            _cinemachineFramingTransposer = GetComponentInChildren<CinemachineFramingTransposer>();
            _defaultOffset = _cinemachineFramingTransposer.m_TrackedObjectOffset.y;

            _minOffset = _defaultOffset - cameraAdjustment;
            _maxOffset = _defaultOffset + cameraAdjustment;
        }
        
        private void Update()
        {
            var move = _controller.GetMove();

            if (move.y != 0)
            {
                _currentDirection = (int)Mathf.Sign(move.y);

                _speedFactor = 1;
                
                if (!_timer.GetProgressStatus())
                {
                    _timer.RestartTimer();
                    _timer.StartTimer();
                }

                if (_lastDirection != _currentDirection)
                {
                    _timer.RestartTimer();
                }

                _lastDirection = _currentDirection;
            }
            else
            {
                _timer.StopTimer();
                _speedFactor = backSpeedFactor;
                _targetOffset = _defaultOffset;
            }

            if (_timer.GetCompleteStatus())
            {
                _targetOffset = _defaultOffset + _currentDirection * cameraAdjustment;
            }
            
            if(Math.Abs(_cinemachineFramingTransposer.m_TrackedObjectOffset.y - _targetOffset) < 0.05) return;

            var direction = Mathf.Sign(_targetOffset - _cinemachineFramingTransposer.m_TrackedObjectOffset.y);
            _cinemachineFramingTransposer.m_TrackedObjectOffset.y += speed * direction * Time.deltaTime * _speedFactor;
            _cinemachineFramingTransposer.m_TrackedObjectOffset.y =
                Mathf.Clamp(_cinemachineFramingTransposer.m_TrackedObjectOffset.y, _minOffset, _maxOffset);
        }
    }
}