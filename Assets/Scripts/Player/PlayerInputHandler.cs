using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
    }

    private void OnEnable()
    {

        //_playerControls.Player.Attack.started += Attack;

        //_playerControls.Player.AutoAttack.started += SwitchAuto;
    }

    private void OnDisable()
    {

        //_playerControls.Player.Attack.started -= Attack;
        
        //_playerControls.Player.AutoAttack.started -= SwitchAuto;
    }

    public Vector2 MoveInput()
    {
        //Debug.Log(playerControls.Player.Move.ReadValue<Vector2>());
        return playerControls.Player.Move.ReadValue<Vector2>();
    }
}
