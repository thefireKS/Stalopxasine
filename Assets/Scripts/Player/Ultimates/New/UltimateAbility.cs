using UnityEngine;

public abstract class UltimateAbility : MonoBehaviour
{
    public int fullEnergy;
    public float ultimateEventTime;
    
    protected PlayerControls _playerControls;
    public abstract void Initialize();

    public abstract void Activate();
    
    protected void PlaySound () {}
}
