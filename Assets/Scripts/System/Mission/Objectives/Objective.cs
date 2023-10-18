using UnityEngine;

namespace System.Mission.Objectives
{
    public abstract class Objective : MonoBehaviour
    {
        public event Action OnCompleted;

        protected abstract void CheckComplete();
        
        protected void Complete()
        {
            OnCompleted?.Invoke();
        }
    }
}
