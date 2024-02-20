using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    //[SerializeField] private KitchenObjectSO kitchenObjectSO;
    private void Update()
    {
        
    }
    public override void Interact(Pemain pemain)
    {
        
        // condition for the clear counter is there kitchen object there
        if (!HasKitchenObject())
        {
            // dan jika pemain memiliki kitchen object, maka kitchen object pada pemain di pindahkan ke clear counter
            if (pemain.HasKitchenObject())
            {
                pemain.GetKitchenObject().SetKitchenObjectParent(this);
            }
            // dan jika pemain tidak memiliki kitchen object 
            else
            {
                
            }
        }
        // if there is kitchen object on a clear counter
        else
        {
            // and if pemain has kitchen object
            if (pemain.HasKitchenObject())
            {
                // jika kitchen object yang di pegang pemain adalah piring
                if (pemain.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // dapatkan kitchen object yang dipegang pemain sebagai plateKitchenObject
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        
                        GetKitchenObject().DestroySelf();
                    }
                }
                // jika pemain tidak memegang piring
                else
                {
                    // jika piring ada pada clear counter
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(pemain.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            pemain.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            // and if pemain did not have the kitchen object, kitchen object on this clear counter move to the pemain
            else
            {
                GetKitchenObject().SetKitchenObjectParent(pemain);
            }
        }
        
    }

}
