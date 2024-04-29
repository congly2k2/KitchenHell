using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameBase
{
    public class GameInput : MonoBehaviour
    {
        private const string PlayerPrefsBinding = "InputBindings";
        public static GameInput Instance { get; private set; }
        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;

        public enum Binding
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
            Interact,
            InteractAlternate,
            Pause
        }
    
        private PlayerInputActions playerInputActions;
        private void Awake()
        {
            GameInput.Instance = this;
            
            this.playerInputActions = new PlayerInputActions();
            
            if (PlayerPrefs.HasKey(GameInput.PlayerPrefsBinding))
            {
                this.playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(GameInput.PlayerPrefsBinding));
            }
            
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
            this.OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
        }

        private void Interact_performed(InputAction.CallbackContext obj)
        {
            this.OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = this.playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector = inputVector.normalized;

            return inputVector;
        }

        public string GetBindingText(Binding binding)
        {
            return binding switch
            {
                Binding.MoveUp => this.playerInputActions.Player.Move.bindings[1].ToDisplayString(),
                Binding.MoveDown => this.playerInputActions.Player.Move.bindings[2].ToDisplayString(),
                Binding.MoveLeft => this.playerInputActions.Player.Move.bindings[3].ToDisplayString(),
                Binding.MoveRight => this.playerInputActions.Player.Move.bindings[4].ToDisplayString(),
                Binding.Interact => this.playerInputActions.Player.Interact.bindings[0].ToDisplayString(),
                Binding.InteractAlternate => this.playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
                Binding.Pause => this.playerInputActions.Player.Pause.bindings[0].ToDisplayString(),
                _ => throw new ArgumentOutOfRangeException(nameof(binding), binding, null)
            };
        }

        public void RebindBinding(Binding binding, Action onActionRebound)
        {
            this.playerInputActions.Player.Disable();

            InputAction inputActions;
            int bindingIndex;

            switch (binding)
            {
                case Binding.MoveUp:
                    inputActions = this.playerInputActions.Player.Move;
                    bindingIndex = 1;
                    break;
                case Binding.MoveDown:
                    inputActions = this.playerInputActions.Player.Move;
                    bindingIndex = 2;
                    break;
                case Binding.MoveLeft:
                    inputActions = this.playerInputActions.Player.Move;
                    bindingIndex = 3;
                    break;
                case Binding.MoveRight:
                    inputActions = this.playerInputActions.Player.Move;
                    bindingIndex = 4;
                    break;
                case Binding.Interact:
                    inputActions = this.playerInputActions.Player.Interact;
                    bindingIndex = 0;
                    break;
                case Binding.InteractAlternate:
                    inputActions = this.playerInputActions.Player.InteractAlternate;
                    bindingIndex = 0;
                    break;
                case Binding.Pause:
                    inputActions = this.playerInputActions.Player.Pause;
                    bindingIndex = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(binding), binding, null);
            }

            inputActions.PerformInteractiveRebinding(bindingIndex)
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    this.playerInputActions.Player.Enable();
                    onActionRebound();
                    
                    PlayerPrefs.SetString(GameInput.PlayerPrefsBinding, this.playerInputActions.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();
                })
                .Start();
        }
    }
}