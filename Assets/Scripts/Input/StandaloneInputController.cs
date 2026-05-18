using System;
using UnityEngine;
using UnityEngine.InputSystem;
using ProjectNahual.Utils;

namespace ProjectNahual.Input
{
    public class StandaloneInputController : MonoBehaviour, InputSystem_Actions.IPlayerActions, InputSystem_Actions.IUIActions, 
    IMenuInput, IPlayerInput
    {
        //Player Inputs
        public Vector2 Move { get => _movementValue; }
        public Vector2 Look { get => _lookValue; }
        public event Action JumpPressed;
        public event Action CrouchPressed;
        public event Action ShootPressed;
        public bool SprintHold { get; set;}
        private Vector2 _movementValue, _lookValue;

        //UI Menu Inputs
        public event Action PausePressed;
        public event Action BackPressed;
        public event Action AcceptPressed;

        private InputSystem_Actions inputActions;

        private void Awake()
        {
            Registry<IPlayerInput>.TryAdd(this);
            Registry<IMenuInput>.TryAdd(this);
        }

        private void OnDestroy()
        {
            Registry<IMenuInput>.Remove(this);
            Registry<IPlayerInput>.Remove(this);
        }

        private void OnEnable()
        {
            inputActions = new InputSystem_Actions();
            inputActions.Player.AddCallbacks(this);
            inputActions.Player.Enable();
            inputActions.UI.AddCallbacks(this);
            inputActions.UI.Enable();
        }
        private void OnDisable()
        {
            inputActions.Player.Disable();
            inputActions.UI.Disable();
        }

        // Input System callback implementations
        public void OnMove(InputAction.CallbackContext context) { _movementValue = context.ReadValue<Vector2>(); }
        public void OnLook(InputAction.CallbackContext context) { _lookValue = context.ReadValue<Vector2>(); }

        public void OnShoot(InputAction.CallbackContext context) { 
            if(context.performed)
            {
                ShootPressed?.Invoke();
            }
        }
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                CrouchPressed?.Invoke();
            }
        }
        public void OnJump(InputAction.CallbackContext context) { 
            if(context.performed)
            {
                JumpPressed?.Invoke();
            }
        }
        public void OnSprint(InputAction.CallbackContext context) { SprintHold = context.performed; }

        public void OnNavigate(InputAction.CallbackContext context) { }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                AcceptPressed?.Invoke();
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                BackPressed?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                PausePressed?.Invoke();
            }
        }
    }
}