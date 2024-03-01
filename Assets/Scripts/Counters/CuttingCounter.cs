using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgressBarUI
{
    // Array fill with recipe kitchen object that can be cut
    [SerializeField] private CutRecipeSO[] cutKitchenObjectSOArray;
    
    private int cuttingProgres;
    public event EventHandler<IHasProgressBarUI.OnProgressChangeEventArgs> OnProgressChange;

    public override void Interact(Pemain pemain)
    {
        
        // Condition for the clear counter is there kitchen object there
        if (!HasKitchenObject())
        {
            // And if pemain has a kitchen object set kitchen object to this object
            if (pemain.HasKitchenObject())
            {
                pemain.GetKitchenObject().SetKitchenObjectParent(this);
                
            }
            // And if pemain did'nt have GameObject
            else
            {

            }
        }
        // If there is kitchen object on a clear counter
        else
        {
            // And if pemain has kitchen object
            if (pemain.HasKitchenObject())
            {
                // If pemain.GetKitchenObject()TryGetPlate is True ( Is a plate ) output reference plateKitchenObject
                if (pemain.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // If plateKitchenObject.TryAddIngredient Is True and add ingredient to List
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
        // If this GameObject has a kitchen object and function HasRecipe() return true
        if (HasKitchenObject() && HasRecipe(GetKitchenObject().GetKitchenObjectSO()))
        {
            // Adding cutting counter from 0 -> 4
            cuttingProgres++;

            // Getting reference CutRecipeSO so later we can get cutRecipeSO.cuttingCountProgres
            CutRecipeSO cutRecipeSO = GetCutRecipeSO(GetKitchenObject().GetKitchenObjectSO());

            // Firing event if the cuttingProgress is added
            OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
            {
                // progresNormalized is cuttingProgres now divided by cutRecipeSO.cuttingCountProgres ( Maximum cut amount )
                // so progresNormalized is percentage of cuttingProgres
                progresNormalized = (float)cuttingProgres / cutRecipeSO.cuttingCountProgres

            });
            // If cuttingProgres is equal or more than cutRecipeSO.cuttingCountProgres ( Maximum cut amount )
            if (cuttingProgres >= cutRecipeSO.cuttingCountProgres)
            {
                // Getting kitchen object using function GetCutItemRecipeFromSO
                KitchenObjectSO cutKitchenObjectSO = GetCutItemRecipeFromSO(GetKitchenObject().GetKitchenObjectSO());

                // Destroy kitchen object on this counter
                GetKitchenObject().DestroySelf();

                // Spawn the kitchen object just we get from function GetCutItemRecipeFromSO
                KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
                cuttingProgres = 0;

                // Firing event to make progresNormalized back to 0
                OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
                {
                    progresNormalized = (float)cuttingProgres / cutRecipeSO.cuttingCountProgres

                });
            }
        }
    }

    private KitchenObjectSO GetCutItemRecipeFromSO(KitchenObjectSO kitchenObjectSO)
    {
        // Get cutRecipeSO from function  GetCutRecipeSO
        CutRecipeSO cutRecipeSO = GetCutRecipeSO(kitchenObjectSO);

        // CutRecipeSO.input equal to function parameter kitchenObjectSO ( example tomato )
        if (cutRecipeSO.input == kitchenObjectSO)
        {
            // Return the cutRecipeSO.output ( example sliced tomato )
            return cutRecipeSO.output;
        }
        return null;
        
    }

    private bool HasRecipe(KitchenObjectSO kitchen)
    {
        // Get cutRecipeSO from function GetCutRecipeSO
        CutRecipeSO cutRecipeSO = GetCutRecipeSO(kitchen);
        // CutRecipeSO.input equal to function parameter kitchenObjectSO ( example tomato )
        if (cutRecipeSO.input == kitchen)
        {
            // Return true
            return true;
        }
        return false;
    }

    private CutRecipeSO GetCutRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        // Creating empty variable to store variable type CutRecipeSO
        CutRecipeSO cutRecipeAny = null;

        // For each cutRecipeSOin array
        foreach (CutRecipeSO cutRecipeSO in cutKitchenObjectSOArray)
        {
            // Fill the cutRecipeAny per iteration with cutRecipeSO
            cutRecipeAny = cutRecipeSO;

            // If kitchenObjectSO is equal to the cut recipe ( example Tomato == Tomato )
            if (cutRecipeSO.input == kitchenObjectSO)
            {
                // Return that cutRecipeSO
                return cutRecipeSO;
            }
        }
        // If not return the last iteration Of The Recipe
        return cutRecipeAny;
    }

}
