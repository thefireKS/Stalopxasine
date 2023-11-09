using Player;
using UnityEngine;

namespace Cinemachine
{
    public class MoveUpDown : MonoBehaviour
    {
        private Controller _controller;

        private int _lastDirection;
        private int _currentDirection;
        
        private CameraPriorityControl _cameraPriorityControl;
        
        [SerializeField] private float timeToStartMove;
        private Timer _timer;

        private void Start()
        {
            _cameraPriorityControl = FindObjectOfType<CameraPriorityControl>();
            
            _controller = FindObjectOfType<Controller>();

            _timer = gameObject.AddComponent<Timer>();
            _timer.SetTime(timeToStartMove);
        }
        
        private void Update()
        {
            var move = _controller.GetMove();

            if (move.y != 0)
            {
                _currentDirection = (int)Mathf.Sign(move.y);

                if (!_timer.GetProgressStatus())
                {
                    _timer.RestartTimer();
                    _timer.StartTimer();
                }

                if (_timer.GetCompleteStatus())
                {
                    _cameraPriorityControl.SetCamerasPriority(_currentDirection > 0 ? "Up" : "Down");
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

                _cameraPriorityControl.SetCamerasPriority("Gameplay");
            }
        }
    }
}