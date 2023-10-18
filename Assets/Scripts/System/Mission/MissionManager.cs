using System.Mission.Objectives;
using UnityEngine;
using UnityEngine.Events;

namespace System.Mission
{
    public class MissionManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onWin;

        private Objective[] _objectives;

        private uint _numberOfObjectives;
        private uint _completedTasks;

        private void Awake()
        {
            _objectives = FindObjectsOfType<Objective>();
            _numberOfObjectives = (uint)_objectives.Length;

            foreach (var objective in _objectives)
            {
                objective.OnCompleted += AddCompletedTask;
            }
        }

        private void AddCompletedTask()
        {
            _completedTasks++;
            CheckWin();
        }

        private void OnDisable()
        {
            foreach (var objective in _objectives)
            {
                objective.OnCompleted -= AddCompletedTask;
            }
        }

        private void CheckWin()
        {
            if (_numberOfObjectives == _completedTasks)
            {
                Win();
            }
        }

        private void Win()
        {
            onWin.Invoke();
        }
    }
}
