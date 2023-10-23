using System;
using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Interactable
{
    
    public class NPC: Interactable
    {
        private static bool _dialogIsGoing;
        
        private GameObject _dialogWindow;
        private TextMeshProUGUI _dialogueText;
        private LocalizeStringEvent _localizeStringEvent;
        [SerializeField] private LocalizedString[] replicas;

        private Controller _playerController;
        
        public event Action OnDialogueEnd;
        
        [Space(10)]
        
        private Animator _animator;
        private Camera _mainCamera;
    
        private float _originalCameraSize;
        
        private int _currentReplica;

        private const float TypingSpeed = 0.04f;

        private Coroutine _displayTextCoroutine;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _dialogWindow = GameObject.FindGameObjectWithTag("DialogueWindow");
            _dialogueText = _dialogWindow.GetComponentInChildren<TextMeshProUGUI>();
            _localizeStringEvent = _dialogWindow.GetComponentInChildren<LocalizeStringEvent>();

            _playerController = FindObjectOfType<Controller>();
        }
        private void Start()
        {
            _dialogWindow.SetActive(false);
            _mainCamera = Camera.main;
            if (_mainCamera != null) _originalCameraSize = _mainCamera.orthographicSize;
            _animator.SetBool("isTalking", false);
        }
        private void CameraZoom()
        {
            if (_mainCamera.orthographicSize == _originalCameraSize)
                _mainCamera.orthographicSize /= 2;
            else
                _mainCamera.orthographicSize *= 2;
        }
        
        public override void ContinuousInteract()
        {
            base.ContinuousInteract();
            DialogueInteraction();
            CameraZoom();
        }
        
        private void DialogueInteraction()
        {
            if (!_dialogIsGoing)
            {
                SetupDialogue(true);
                _localizeStringEvent.StringReference = replicas[_currentReplica];
                _displayTextCoroutine = StartCoroutine(DisplayLine());
            }
            else
            {
                if (_displayTextCoroutine != null)
                {
                    StopCoroutine(_displayTextCoroutine);
                    _dialogueText.maxVisibleCharacters = _dialogueText.text.Length;
                    _displayTextCoroutine = null;
                    return;
                }

                _currentReplica++;
                if (_currentReplica >= replicas.Length)
                {
                    _currentReplica = 0;
                    SetupDialogue(false);
                }

                _localizeStringEvent.StringReference = replicas[_currentReplica];
                _displayTextCoroutine = StartCoroutine(DisplayLine());
            }
        }

        private IEnumerator DisplayLine()
        {
            _dialogueText.maxVisibleCharacters = 0;

            var isAddingRichTextTag = false;

            foreach (var letter in _dialogueText.text.ToCharArray())
            {
                if (letter == '<' || isAddingRichTextTag)
                {
                    isAddingRichTextTag = true;
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                    }

                    _dialogueText.maxVisibleCharacters++;
                }
                else
                {
                    _dialogueText.maxVisibleCharacters++;
                    yield return new WaitForSeconds(TypingSpeed);
                }
            }
            _displayTextCoroutine = null;
        }

        private void SetupDialogue(bool dialogueState)//хз может оно и не надо, но я решил вынести
        {
            _dialogIsGoing = dialogueState;
            _dialogWindow.SetActive(dialogueState);
            _animator.SetBool("isTalking", dialogueState);
            
            if (dialogueState)
            {
                _playerController.ChangeActionState(Controller.ActionStates.Dialogue);
            }
            else
            {
                _playerController.ChangeActionState(Controller.ActionStates.Idle);
                OnDialogueEnd?.Invoke();
            }
        }
    }
}