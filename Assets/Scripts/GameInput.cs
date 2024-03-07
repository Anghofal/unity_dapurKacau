using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    // Get the reference from the auto generated input actions script
    private PlayerInputActions inputActions;

    // Creating event so later we can fire the event on button is pressed
    public event EventHandler OnInteractActions;
    public event EventHandler OnAlternateInteract;


    private void Awake()
    {
        // Initialize the script
        inputActions = new PlayerInputActions();
        
        // Enabling the action maps
        inputActions.Player.Enable();

        // We add Interact_performed function to inputActions.Player.Interact.performed as a delegate
        // So when inputActions.Player.Interact.performed is called we also called the function
        inputActions.Player.Interact.performed += Interact_performed;

        // We add AlternateInteract_performed function to inputActions.Player.AlternateInteract.performed as a delegate
        inputActions.Player.AlternateInteract.performed += AlternateInteract_performed;
    }

    private void AlternateInteract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // Firing the event
        OnAlternateInteract?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // Firing the event
        OnInteractActions?.Invoke(this, EventArgs.Empty);
        
    }

    public Vector2 GetMovementVectorNormalized()
    {
        // Read input value from the bbutton pressed
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        
        // Normalize the vector so when moving on diagonal vector the speed is not doubled
        // Because when pemain is moving on diagonal the position of x + 1 and z + 1 so the speed is doubled
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
