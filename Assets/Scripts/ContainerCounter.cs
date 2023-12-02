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
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            kitchenObjectTransform.localPosition = Vector3.zero;
            // got the reference from game object that we just spawned
            KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            kitchenObject.SetKitchenObjectParent(pemain);

            OnPlayerGrabKitchenObject?.Invoke(this, EventArgs.Empty);
        }
    }

   
}
