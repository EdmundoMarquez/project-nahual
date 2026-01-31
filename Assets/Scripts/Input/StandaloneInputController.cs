using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectNahual.Input
{
    public class StandaloneInputController : MonoBehaviour, InputSystem_Actions.IPlayerActions, IPlayerInput
    {
        public Vector2 Move { get => _movementValue; }
        public Vector2 Look { get => _lookValue; }
        public event Action JumpPressed;
        public event Action CrouchPressed;
        public event Action ShootPressed;
        public bool SprintHold { get; set;}
        private Vector2 _movementValue, _lookValue;

        private InputSystem_Actions inputActions;

        private void OnEnable()
        {
            inputActions = new InputSystem_Actions();
            inputActions.Player.AddCallbacks(this);
            inputActions.Player.Enable();
        }
        private void OnDisable()
        {
            inputActions.Player.Disable();
        }

        // Input System callback implementations
        public void OnMove(InputAction.CallbackContext context) { _movementValue = context.ReadValue<Vector2>(); }
        public void OnLook(InputAction.CallbackContext context) { _lookValue = context.ReadValue<Vector2>(); }
        public void OnAttack(InputAction.CallbackContext context) { 
            if(context.performed)
            {
                ShootPressed?.Invoke();
            }
        }
        public void OnInteract(InputAction.CallbackContext context) { }
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                Debug.Log("Is crouching");
                CrouchPressed?.Invoke();
            }
        }
        public void OnJump(InputAction.CallbackContext context) { 
            if(context.performed)
            {
                JumpPressed?.Invoke();
            }
        }
        public void OnPrevious(InputAction.CallbackContext context) { }
        public void OnNext(InputAction.CallbackContext context) { }
        public void OnSprint(InputAction.CallbackContext context) { SprintHold = context.performed; }
    }
}