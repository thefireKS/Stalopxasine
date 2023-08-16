using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
    }

    private void OnEnable()
    {
        _playerControls.Player.Jump.started += JumpStart;
        _playerControls.Player.Jump.performed += JumpEnd;
        _playerControls.Player.Jump.canceled += JumpEnd;
        
        //_playerControls.Player.Attack.started += Attack;

        //_playerControls.Player.AutoAttack.started += SwitchAuto;
    }

    private void OnDisable()
    {
        _playerControls.Player.Jump.started -= JumpStart;
        _playerControls.Player.Jump.performed -= JumpEnd;
        _playerControls.Player.Jump.canceled -= JumpEnd;
        
        //_playerControls.Player.Attack.started -= Attack;
        
        //_playerControls.Player.AutoAttack.started -= SwitchAuto;
    }

    private void JumpStart(InputAction.CallbackContext context)
    {
        
    }

    private void JumpEnd(InputAction.CallbackContext context)
    {
        
    }

    public Vector2 MoveInput()
    {
        return _playerControls.Player.Move.ReadValue<Vector2>();
    }
}
