using System;
using System.Collections;
using Interactable;
using Player.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Health : MonoBehaviour, IDamageable
    {
        public void Initialize(int maxHealth)
        {
            SetMaxHealth(maxHealth);
        }
    
        private Animator _animator;
        private CameraShaking _cameraShaking; 
        
        private ParticleSystem _particleSystem;
    
        private bool _isImmortal;
        private int _currentHealth;
        private int _maxHealth;
        private const float TakingDamageTime = 0.5f;
        
        private void SetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public static event Action<int> OnHealthChanged;

        private readonly WaitForSeconds _takingDamage = new(TakingDamageTime);

        private void OnEnable()
        {
            ActionState.OnActionStateChanged += ProcessStateChange;
        }

        private void OnDisable()
        {
            ActionState.OnActionStateChanged -= ProcessStateChange;
        }

        private void Start()
        {
            SetHealth(_maxHealth);
            OnHealthChanged?.Invoke(_maxHealth);
            _animator = GetComponentInChildren<Animator>();
            if (Camera.main != null) _cameraShaking = Camera.main.GetComponent<CameraShaking>();
            _particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
            _particleSystem.Stop();
        }

        private void CheckHealth()
        {
            if (_currentHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                _particleSystem.Stop();
            }
               
        }

        private void SetHealth(int value)
        {
            _currentHealth = value;
            OnHealthChanged?.Invoke(_currentHealth);
        }

        private IEnumerator GotDamaged(int damage)
        {
            _currentHealth-=damage;
            CheckHealth();
            OnHealthChanged?.Invoke(_currentHealth);
            _isImmortal = true;
            _animator.SetBool("isHitted", true);
            yield return _takingDamage;
            _animator.SetBool("isHitted", false);
            _isImmortal = false;
        }

        public void TakeDamage(int dmg)
        {
            if (!_isImmortal)
            {
                StartCoroutine(GotDamaged(dmg));
                _cameraShaking.Shake(TakingDamageTime, 2f);
                _particleSystem.Play();
            }
        }

        private void ProcessStateChange(ActionState.States actionStates)
        {
            _isImmortal = actionStates == ActionState.States.Dialogue;
        }
    }
}