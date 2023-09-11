using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerControls PlayerControls;

    private void Awake()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();
    }

    public static event Action Interaction;

    private void InvokeInteraction(InputAction.CallbackContext callbackContext)
    {
        Interaction?.Invoke();
    }

    private void OnEnable()
    {
        PlayerControls.Player.Interact.performed += InvokeInteraction;

        //_playerControls.Player.Attack.started += Attack;

        //_playerControls.Player.AutoAttack.started += SwitchAuto;
    }

    private void OnDisable()
    {
        PlayerControls.Player.Interact.performed -= InvokeInteraction;

        //_playerControls.Player.Attack.started -= Attack;
        
        //_playerControls.Player.AutoAttack.started -= SwitchAuto;
    }

    public Vector2 MoveInput()
    {
        //Debug.Log(playerControls.Player.Move.ReadValue<Vector2>());
        return PlayerControls.Player.Move.ReadValue<Vector2>();
    }
}
