using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgressBarUI
{
    // Array berisi kitchenObjectSO dari recipe sayuran ke potongan sayuran
    [SerializeField] private CutRecipeSO[] cutKitchenObjectSOArray;
    private int cuttingProgres;
    public event EventHandler<IHasProgressBarUI.OnProgressChangeEventArgs> OnProgressChange;

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
            }
            // and if pemain did not have the kitchen object, kitchen object on this clear counter move to the pemain
            else
            {
                GetKitchenObject().SetKitchenObjectParent(pemain);
            }
        }

    }

    public override void InteractAlternate(Pemain pemain)
    {
        // jika cutting counter memiliki objek dan HasRecipe() return true
        if (HasKitchenObject() && HasRecipe(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgres++;

            CutRecipeSO cutRecipeSO = GetCutRecipeSO(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
            {
                progresNormalized = (float)cuttingProgres / cutRecipeSO.cuttingCountProgres

            });
            if (cuttingProgres >= cutRecipeSO.cuttingCountProgres)
            {
                KitchenObjectSO cutKitchenObjectSO = GetCutItemRecipeFromSO(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
                cuttingProgres = 0;

                OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
                {
                    progresNormalized = (float)cuttingProgres / cutRecipeSO.cuttingCountProgres

                });
            }
        }
    }

    private KitchenObjectSO GetCutItemRecipeFromSO(KitchenObjectSO kitchen)
    {
        CutRecipeSO cutRecipeSO = GetCutRecipeSO(kitchen);
        if (cutRecipeSO.input == kitchen)
        {
            return cutRecipeSO.output;
        }
        return null;
        
    }

    private bool HasRecipe(KitchenObjectSO kitchen)
    {
        // dapatkan recipe dari fungsi GetCutRecipeSO
        CutRecipeSO cutRecipeSO = GetCutRecipeSO(kitchen);
        // jika recipe.input adalah kitchen
        if (cutRecipeSO.input == kitchen)
        {
            return true;
        }
        return false;
    }

    private CutRecipeSO GetCutRecipeSO(KitchenObjectSO kitchenObjectSO)
    {

        CutRecipeSO cutRecipeAny = null;
        foreach (CutRecipeSO cutRecipeSO in cutKitchenObjectSOArray)
        {
            cutRecipeAny = cutRecipeSO;
            if (cutRecipeSO.input == kitchenObjectSO)
            {
                return cutRecipeSO;
            }
        }
        return cutRecipeAny;
    }

}
