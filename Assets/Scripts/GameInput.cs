using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    // Didapat dari script auto Generated yang akan nanti digunakan untuk mendapatkan Vector2 nya
    private PlayerInputActions inputActions;
    public event EventHandler onInteractActions;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        onInteractActions?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        
        // Dinormalisasikan agar ketika vector nya x + 1 dan z + 1 movement speed nya tidak 2
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
