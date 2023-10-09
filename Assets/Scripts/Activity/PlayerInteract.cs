using UnityEngine;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {

        [SerializeField] private float interactRange = 1f;
        private Animator _animator;
        private Camera mainCamera;

        private float originalCameraSize;
        
        private void OnEnable()
        {
            PlayerInputHandler.Interaction += Interact;
        }

        private void OnDisable()
        {
            PlayerInputHandler.Interaction -= Interact;
        }


        private void Start()
        {
            mainCamera = Camera.main;
            originalCameraSize = mainCamera.orthographicSize;
        }
        private void CameraZoom()
        {
            if (mainCamera.orthographicSize == originalCameraSize)
                mainCamera.orthographicSize /= 2;
            else
                mainCamera.orthographicSize *= 2;
        }
        
        private void Interact()
        {
            var interactable = GetInteractable();
            if (interactable == null) return;
            
            Debug.Log(interactable + "uuu");
            interactable.Interact();
            CameraZoom();
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