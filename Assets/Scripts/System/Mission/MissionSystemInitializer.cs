using System.Mission.UI;
using UnityEngine;

namespace System.Mission
{
    public class MissionSystemInitializer : MonoBehaviour
    {
        [SerializeField] private ObjectivesManager objectivesManager;
        [SerializeField] private MissionUI missionUI;

        private async void Awake()
        {
            await objectivesManager.Initialize();
            await missionUI.Initialize();
            
            Destroy(this);
        }
    }
}
