using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactable
{
    public class Interactable: MonoBehaviour, IInteractable
    {
        private GameObject _player;
        [SerializeField] private float interactRange = 1f;

        private bool _canInteract = false;

        private enum InteractionType
        {
            Quick,
            Continuous
        }
        [SerializeField] private InteractionType interactionType;
        
        private void OnEnable()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            PlayerInputHandler.Interaction += Interact;
            InteractUI.InteractionMarker.Hide();
        }
        private void OnDisable()
        {
            InteractUI.InteractionMarker.Hide();
            PlayerInputHandler.Interaction -= Interact;
        }

        private void Update()
        {
            if (IsPlayerNearby())
            {
                InteractUI.InteractionMarker.Show();
                _canInteract = true;
            }
            else
            {
                InteractUI.InteractionMarker.Hide();
                _canInteract = false;
            }
        }

        public void Interact()
        {
            if(!_canInteract) return;
            
            switch (interactionType)
            {
                case InteractionType.Quick:
                    QuickInteract();
                    break;
                case InteractionType.Continuous:
                    ContinuousInteract();
                    break;
             
                default: 
                    QuickInteract();
                    break;
            }
        }
        public virtual void QuickInteract()
        {
            Debug.Log("AHahahahaha");//test
        }
        public virtual void ContinuousInteract()
        {
            Debug.Log("UwU");//test
        }

        private bool IsPlayerNearby()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
            foreach (var collider in colliders)
            {
                if (collider.gameObject == _player)
                {
                    Debug.Log("Ya nashel tebya");
                    return true;
                }
            }
            return false;
        }
    }
}