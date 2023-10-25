using System.Collections;
using UnityEngine;

namespace System
{
    public class HitImpact : MonoBehaviour
    {
        [SerializeField] private Material flashMaterial;
        
        [SerializeField] private float duration;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Material _originalMaterial;
        private Coroutine _flashRoutine;

        void Start()
        {
            if(!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _originalMaterial = spriteRenderer.material;
        }

        public void Flash()
        {
            if (_flashRoutine != null)
            {
                StopCoroutine(_flashRoutine);
            }
            
            _flashRoutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            spriteRenderer.material = flashMaterial;
            
            yield return new WaitForSeconds(duration);
            
            spriteRenderer.material = _originalMaterial;
            
            _flashRoutine = null;
        }
    }
}
