using System.Mission.Objectives.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace System.Mission.UI
{
    public class ObjectiveUI : MonoBehaviour
    {
        private Objective _objective;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI progressText;

        public void SetObjective(Objective objective)
        {
            _objective = objective;
            Initialize();
        }

        private void OnDisable()
        {
            _objective.OnReady -= null;
            _objective.OnCompleted -= null;
            if (_objective is QuantityObjective quantityObjective)
            {
                quantityObjective.OnCountUpdate -= null;
            }
        }

        private void Initialize()
        {
            _objective.OnReady += UpdateText;
            _objective.OnCompleted += UpdateText;
            if (_objective is QuantityObjective quantityObjective)
            {
                quantityObjective.OnCountUpdate += UpdateText;
            }
            
            image.sprite = _objective.GetObjectiveImage();
            transform.name = _objective.GetObjectiveName();

            Debug.Log($"Objective UI: {name} initialized");
        }

        private void UpdateText()
        {
            if (_objective.isComplete)
            {
                progressText.text = "Completed";
            }
            else
            {
                if (_objective is QuantityObjective quantityObjective)
                {
                    progressText.text = $"{quantityObjective.GetCurrentCount()} / {quantityObjective.GetTargetCount()}";
                }
                else
                {
                    progressText.text = "In Progress";
                }
            }

            Debug.Log($"Objective UI: {name} text updated");
        }
    }
}
