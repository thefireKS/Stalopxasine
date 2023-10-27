using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private PlayerControls _playerControls;

    public static bool IsPaused;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
        
        IsPaused = false;
    }

    private void OnEnable()
    {
        _playerControls.Player.Pause.started += ChangeState;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void OnDisable()
    {
        _playerControls.Player.Pause.started -= ChangeState;
    }
    
    private void ChangeState(InputAction.CallbackContext callbackContext)
    {
        IsPaused = !IsPaused;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0f : 1f;
    }

    public void ChangeState()
    {
        IsPaused = !IsPaused;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0f : 1f;
    }
}
