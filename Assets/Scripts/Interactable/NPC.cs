using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Interactable
{
    
    public class NPC: Interactable
    {
        public static bool DialogIsGoing=false;
        
        private GameObject DialogWindow;
        private TextMeshProUGUI _dialogueText;
        private LocalizeStringEvent _localizeStringEvent;
        [SerializeField] private LocalizedString[] replicas;
        
        [Space(10)] 
        

        private Animator _animator;
        private Camera mainCamera;
    
        private float originalCameraSize;
        
        private int currentReplica = 0;

        private readonly float typingSpeed = 0.04f;
        
        private Coroutine displayTextCoroutine;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            DialogWindow = GameObject.FindGameObjectWithTag("DialogueWindow");
            _dialogueText = DialogWindow.GetComponentInChildren<TextMeshProUGUI>();
            _localizeStringEvent = DialogWindow.GetComponentInChildren<LocalizeStringEvent>();
        }
        private void Start()
        {
            DialogWindow.SetActive(false);
            mainCamera = Camera.main;
            originalCameraSize = mainCamera.orthographicSize;
            _animator.SetBool("isTalking", false);
        }
        private void CameraZoom()
        {
            if (mainCamera.orthographicSize == originalCameraSize)
                mainCamera.orthographicSize /= 2;
            else
                mainCamera.orthographicSize *= 2;
        }
        
        public override void ContinuousInteract()
        {
            base.ContinuousInteract();
            DialogueInteraction();
            CameraZoom();
        }
        
        private void DialogueInteraction()
        {
            if (!DialogIsGoing)
            {
                SetupDialogue(true);
                _localizeStringEvent.StringReference = replicas[currentReplica];
                displayTextCoroutine = StartCoroutine(DisplayLine());
            }
            else
            {
                if (displayTextCoroutine != null)
                {
                    StopCoroutine(displayTextCoroutine);
                    _dialogueText.maxVisibleCharacters = _dialogueText.text.Length;
                    displayTextCoroutine = null;
                    return;
                }

                currentReplica++;
                if (currentReplica >= replicas.Length)
                {
                    currentReplica = 0;
                    SetupDialogue(false);
                }

                _localizeStringEvent.StringReference = replicas[currentReplica];
                displayTextCoroutine = StartCoroutine(DisplayLine());
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
                    yield return new WaitForSeconds(typingSpeed);
                }
            }
            displayTextCoroutine = null;
        }

        private void SetupDialogue(bool dialogueState)//хз может оно и не надо, но я решил вынести
        {
            DialogIsGoing = dialogueState;
            DialogWindow.SetActive(dialogueState);
            _animator.SetBool("isTalking", dialogueState);
        }
    }
}