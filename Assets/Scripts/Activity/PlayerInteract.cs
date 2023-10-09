using System;
using Interactable;
using UnityEngine;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {

        /*[SerializeField] private float interactRange = 1f;

        private IInteractable interactable;
        
        private void OnEnable()
        {
            PlayerInputHandler.Interaction += Interact;
        }
        private void OnDisable()
        {
            PlayerInputHandler.Interaction -= Interact;
        }

        private void Update()
        {
            interactable = GetInteractable();
            
            if (GetInteractable()==null)
                InteractUI.InteractionMarker.Hide();
            else InteractUI.InteractionMarker.Show();
        }

        private void Interact()
        {
            interactable?.Interact(interactable.CurrentInteractionType);
        }

        private IInteractable GetInteractable()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IInteractable npcInteractable))
                {
                    return npcInteractable;
                }
            }
            return null;
        }*/
    }
}