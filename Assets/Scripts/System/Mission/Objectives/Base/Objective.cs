using System.Threading.Tasks;
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

        [SerializeField] private string objectiveName;
        [SerializeField] private Sprite objectiveImage;

        public string GetObjectiveName()
        {
            return objectiveName;
        }
        
        public Sprite GetObjectiveImage()
        {
            return objectiveImage;
        }
        
        public event Action OnCompleted;

        public event Action OnReady;

        public bool isComplete;

        [SerializeField] protected ObjectiveType objectiveType;

        protected abstract void CheckComplete();

        public async void Prepare()
        {
            await Initialize();
            OnReady?.Invoke();
            Debug.Log($"Objective: {objectiveName} ready");
        }

        private Task Initialize()
        {
            InitializeJob();
            return Task.CompletedTask;
        }

        protected virtual void InitializeJob()
        {
            
        }
        
        
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
