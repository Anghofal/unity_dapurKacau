using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Pemain pemain)
    {
        if (pemain.HasKitchenObject()) {
            pemain.GetKitchenObject().DestroySelf();
        }
    }
}
