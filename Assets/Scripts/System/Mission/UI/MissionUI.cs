using System.Collections.Generic;
using System.Mission.Objectives.Base;
using System.Threading.Tasks;
using Interactable;
using UnityEngine;

namespace System.Mission.UI
{
    public class MissionUI : MonoBehaviour
    {
        private enum Corner
        {
            LeftUp,
            LeftDown,
            RightUp,
            RightDown
        }
        
        [SerializeField] private ObjectivesManager objectivesManager;

        [SerializeField] private GameObject objectiveUIPrefab;

        [SerializeField] private RectTransform mainObjectivesHandler, sideObjectivesHandler;
        [SerializeField] private Corner mainObjectivesCorner, sideObjectivesCorner;

        private List<GameObject> missionObjects = new List<GameObject>();

        private void OnEnable()
        {
            NPC.OnDialogueSignal += MissionVisibilityManager;
        }

        private void OnDisable()
        {
            NPC.OnDialogueSignal -= MissionVisibilityManager;
        }

        public Task Initialize()
        {
            foreach (var objective in objectivesManager.GetObjectives())
            {
                switch (objective.GetObjectiveType())
                {
                    case Objective.ObjectiveType.Main:
                        SpawnObjective(mainObjectivesHandler, objective);
                        break;
                    case Objective.ObjectiveType.Side:
                        SpawnObjective(sideObjectivesHandler, objective);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Debug.Log("Mission UI: Spawn all ObjectiveUI");
            
            SetPosition(mainObjectivesHandler, mainObjectivesCorner);
            SetPosition(sideObjectivesHandler, sideObjectivesCorner);
            
            return Task.CompletedTask;
        }

        private void SpawnObjective(Transform parent, Objective objective)
        {
            var objectiveUI = Instantiate(objectiveUIPrefab, parent).GetComponent<ObjectiveUI>();
            objectiveUI.SetObjective(objective);
            missionObjects.Add(objectiveUI.gameObject);
        }

        private void SetPosition(RectTransform rectTransform,Corner corner)
        {
            switch (corner)
            {
                case Corner.LeftUp:
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    rectTransform.pivot = new Vector2(0,1);
                    break;
                case Corner.LeftDown:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    rectTransform.pivot = new Vector2(0,0);
                    break;
                case Corner.RightUp:
                    rectTransform.anchorMin = new Vector2(1, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    rectTransform.pivot = new Vector2(1,1);
                    break;
                case Corner.RightDown:
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    rectTransform.pivot = new Vector2(1,0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(corner), corner, null);
            }
        }

        private void MissionVisibilityManager(bool shouldBeDisabled)
        {
            foreach (var missionObject in missionObjects)
            {
                missionObject.SetActive(shouldBeDisabled);
                Debug.Log(missionObject);
            }
        }
    }
}
