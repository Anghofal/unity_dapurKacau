using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    // Contain of spesific kitchen object to spawn later
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabKitchenObject;

    public override void Interact(Pemain pemain)
    {
        // If pemain is not carrying anything
        if (!pemain.HasKitchenObject())
        {
            // Spawn the spesific kitchen object
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, pemain);

            // Firing the event for the animation of container counter
            OnPlayerGrabKitchenObject?.Invoke(this, EventArgs.Empty);
        }
    }

   
}
