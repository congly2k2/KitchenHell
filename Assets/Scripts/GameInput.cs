using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        this.playerInputActions = new PlayerInputActions();
        this.playerInputActions.Player.Enable();

        this.playerInputActions.Player.Interact.performed += Interact_performed;
        this.playerInputActions.Player.InteractAlternate.performed += InteractAlternateOnPerformed;
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