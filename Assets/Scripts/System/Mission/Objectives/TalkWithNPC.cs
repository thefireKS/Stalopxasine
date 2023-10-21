using System.Mission.Objectives.Base;
using Interactable;
using UnityEngine;

namespace System.Mission.Objectives
{
    public class TalkWithNpc : Objective
    {
        [SerializeField] private NPC npc;

        private void OnEnable()
        {
            npc.OnDialogueEnd += CheckComplete;
        }

        private void OnDisable()
        {
            npc.OnDialogueEnd -= CheckComplete;
        }

        protected override void CheckComplete()
        {
            if(isComplete) return;
            Complete();
        }

        
    }
}
