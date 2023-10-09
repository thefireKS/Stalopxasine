using UnityEngine;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {

        [SerializeField] private float interactRange = 1f;
        
        private void OnEnable()
        {
            PlayerInputHandler.Interaction += Interact;
        }
        private void OnDisable()
        {
            PlayerInputHandler.Interaction -= Interact;
        }
        
        private void Interact()
        {
            var interactable = GetInteractable();
            
            interactable?.Interact(interactable.CurrentInteractionType);
        }

        public IInteractable GetInteractable()
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
        }
    }
}