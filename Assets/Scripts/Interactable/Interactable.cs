using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactable
{
    public class Interactable: MonoBehaviour, IInteractable
    {
        private GameObject _player;
        [SerializeField] private float interactRange = 1f;
        private InteractUI _interactionMarker;

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
            _interactionMarker = GetComponentInChildren<InteractUI>();
            _interactionMarker.Hide();
        }
        private void OnDisable()
        {
            _interactionMarker.Hide();
            PlayerInputHandler.Interaction -= Interact;
        }

        private void Update()
        {
            if (IsPlayerNearby())
            {
                _interactionMarker.Show();
                _canInteract = true;
            }
            else
            {
                _interactionMarker.Hide();
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
                    return true;
                }
            }
            return false;
        }
    }
}