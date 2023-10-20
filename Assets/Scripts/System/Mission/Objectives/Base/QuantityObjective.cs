using UnityEngine;

namespace System.Mission.Objectives.Base
{
    public class QuantityObjective : Objective
    {
        protected uint _targetCount;
        protected uint _currentCount;

        public event Action OnCountUpdate;
        
        public uint GetCurrentCount()
        {
            return _currentCount;
        }

        public uint GetTargetCount()
        {
            return _targetCount;
        }

        public float GetCompletePercentage()
        {
            return (float) _currentCount / _targetCount * 100f;
        }

        protected void AddCount()
        {
            _currentCount++;
            OnCountUpdate?.Invoke();
            Debug.Log(GetCompletePercentage());
            CheckComplete();
        }
        
        protected override void CheckComplete()
        {
            if (_currentCount == _targetCount)
            {
                Complete();
            }
        }
    }
}
