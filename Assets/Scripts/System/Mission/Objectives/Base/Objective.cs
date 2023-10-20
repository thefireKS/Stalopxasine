using UnityEngine;

namespace System.Mission.Objectives.Base
{
    public abstract class Objective : MonoBehaviour
    {
        public enum ObjectiveType
        {
            Main,
            Side
        }
        
        public event Action OnCompleted;

        public bool isComplete;

        [SerializeField] protected ObjectiveType objectiveType;

        protected abstract void CheckComplete();
        
        protected void Complete()
        {
            isComplete = true;
            OnCompleted?.Invoke();
        }

        public ObjectiveType GetObjectiveType()
        {
            return objectiveType;
        }
    }
}
