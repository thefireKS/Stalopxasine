using System.Mission.Objectives.Base;
using UnityEngine;
using UnityEngine.Events;

namespace System.Mission
{
    public class ObjectivesManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onCompleteMainObjectives;
        [SerializeField] private UnityEvent onCompleteSideObjectives;

        private Objective[] _objectives;
        private uint _countIsReady;

        public Objective[] GetObjectives()
        {
            return _objectives;
        }

        private uint _numberOfMainObjectives;
        private uint _completedMainObjectives;
        
        private uint _numberOfSideObjectives;
        private uint _completedSideObjectives;

        public static event Action OnFindObjectives;

        private void Awake()
        {
            _objectives = FindObjectsOfType<Objective>();
            OnFindObjectives?.Invoke();
            
            foreach (var objective in _objectives)
            {
                switch (objective.GetObjectiveType())
                {
                    case Objective.ObjectiveType.Main:
                        objective.OnCompleted += IncreaseCompletedMainObjectives;
                        _numberOfMainObjectives++;
                        break;
                    case Objective.ObjectiveType.Side:
                        objective.OnCompleted += IncreaseCompletedSideObjectives;
                        _numberOfSideObjectives++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void IncreaseCompletedMainObjectives()
        {
            _completedMainObjectives++;
            CheckMainObjectives();
        }

        private void IncreaseCompletedSideObjectives()
        {
            _completedSideObjectives++;
            CheckSideObjectives();
        }

        private void OnDisable()
        {
            foreach (var objective in _objectives)
            {
                objective.OnCompleted -= null;
            }
        }

        private void CheckMainObjectives()
        {
            if (_completedMainObjectives == _numberOfMainObjectives)
            {
                OnMainObjectivesCompleted();
            }
        }

        private void OnMainObjectivesCompleted()
        {
            onCompleteMainObjectives.Invoke();
        }
        private void CheckSideObjectives()
        {
            if (_completedSideObjectives == _numberOfSideObjectives)
            {
                OnSideObjectivesCompleted();
            }
        }

        private void OnSideObjectivesCompleted()
        {
            onCompleteSideObjectives.Invoke();
        }
    }
}
