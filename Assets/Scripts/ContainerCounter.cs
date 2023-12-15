using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnPlayerGrabKitchenObject;

    public override void Interact(Pemain pemain)
    {
        if (!pemain.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, pemain);

            OnPlayerGrabKitchenObject?.Invoke(this, EventArgs.Empty);
        }
    }

   
}
