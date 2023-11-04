using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _targetTime;
    private float _currentTime;

    private bool _inProgress;
    private bool _isComplete;

    private void Update()
    {
        if (_inProgress && !_isComplete)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                _isComplete = true;
            }
        }
    }

    public void RestartTimer()
    {
        _currentTime = _targetTime;
        _isComplete = false;
    }

    public void StartTimer()
    {
        _inProgress = true;
    }

    public void StopTimer()
    {
        _inProgress = false;
        _isComplete = false;
    }

    public bool GetCompleteStatus()
    {
        return _isComplete;
    }
    
    public bool GetProgressStatus()
    {
        return _inProgress;
    }

    public void SetTime(float time)
    {
        _targetTime = time;
    }
}
