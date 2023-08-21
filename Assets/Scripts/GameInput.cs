using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    // Didapat dari script auto Generated yang akan nanti digunakan untuk mendapatkan Vector2 nya
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        
        inputActions.Player.Enable();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        
        // Dinormalisasikan agar ketika vector nya x + 1 dan z + 1 movement speed nya tidak 2
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
