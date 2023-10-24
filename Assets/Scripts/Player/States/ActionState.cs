using System;
using UnityEngine;

namespace Player.States
{
    public class ActionState : MonoBehaviour
    {
        public enum States
        {
            Idle,
            Attacking,
            Dialogue
        }
        
        public static event Action<States> OnActionStateChanged;

        private States _state = States.Idle;
        
        public void ChangeActionState(States newState)
        {
            _state = newState;
            OnActionStateChanged?.Invoke(_state);
        }

        public States GetState()
        {
            return _state;
        }
    }
}
