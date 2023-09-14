using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class PlayerMeeting : MonoBehaviour
{
    [SerializeField] private string afkAnimName;
    [SerializeField] private string GreetingAnimName;
    
    public static bool DialogIsGoing=false;

    [SerializeField] private GameObject DialogWindow;
    [Space(10)] 
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private LocalizeStringEvent _localizeStringEvent;
    [SerializeField] private LocalizedString[] replicas;

    private Animator _animator;
    private Camera mainCamera;
    
    private float originalCameraSize;
    private bool inZone=false;
    private int currentReplica = 0;

    private readonly float typingSpeed = 0.04f;
    private GameObject player;
    private Coroutine displayTextCoroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    

    private void Start()
    {
        mainCamera = Camera.main;
        originalCameraSize = mainCamera.orthographicSize;
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnEnable()
    {
        PlayerInputHandler.Interaction += DialogueInteraction;
    }

    private void OnDisable()
    { 
        PlayerInputHandler.Interaction -= DialogueInteraction;
    }

    private void Update()
    {
        if(inZone && !DialogIsGoing)
            _animator.Play(afkAnimName);
    }

    #region Dialogue System

    private void DialogueInteraction()
    {
        if (!inZone) return;

        if (!DialogIsGoing)
        {
            DialogIsGoing = true;
            _animator.Play(GreetingAnimName);
            DialogWindow.SetActive(true);
            _localizeStringEvent.StringReference = replicas[currentReplica];
            displayTextCoroutine = StartCoroutine(DisplayLine());
            CameraZoom();
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
                currentReplica = 0;

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

    public void DialogEnd()
    {
        DialogIsGoing = false;
        CameraZoom();
    }
    
    #endregion
    
    private void CameraZoom()
    {
        if(mainCamera.orthographicSize == originalCameraSize)
            mainCamera.orthographicSize /= 2;
        else
            mainCamera.orthographicSize *= 2;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != player) return;
        inZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != player) return;
        inZone = false;
        _animator.Play(afkAnimName);
    }
}