using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameBase
{
    public class GameInput : MonoBehaviour
    {
        public static GameInput Instance { get; private set; }
        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;
    
        private PlayerInputActions playerInputActions;
        private void Awake()
        {
            GameInput.Instance = this;
            
            this.playerInputActions = new PlayerInputActions();
            this.playerInputActions.Player.Enable();

            this.playerInputActions.Player.Interact.performed += this.Interact_performed;
            this.playerInputActions.Player.InteractAlternate.performed += this.InteractAlternateOnPerformed;
            this.playerInputActions.Player.Pause.performed += this.Pause_performed;
        }

        private void OnDestroy()
        {
            this.playerInputActions.Player.Interact.performed -= this.Interact_performed;
            this.playerInputActions.Player.InteractAlternate.performed -= this.InteractAlternateOnPerformed;
            this.playerInputActions.Player.Pause.performed -= this.Pause_performed;
            
            this.playerInputActions.Dispose();
        }

        private void Pause_performed(InputAction.CallbackContext obj)
        {
            this.OnPauseAction?.Invoke(this, EventArgs.Empty);
        }

        private void InteractAlternateOnPerformed(InputAction.CallbackContext obj)
        {
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
        }

        private void Interact_performed(InputAction.CallbackContext obj)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = this.playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector = inputVector.normalized;
            // Debug.Log(inputVector);

            return inputVector;
        }
    }
}