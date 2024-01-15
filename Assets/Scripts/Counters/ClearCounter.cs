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
        Debug.Log("berhasil interaksi");
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

            }
            // and if pemain did not have the kitchen object, kitchen object on this clear counter move to the pemain
            else
            {
                GetKitchenObject().SetKitchenObjectParent(pemain);
            }
        }
        
    }

}
