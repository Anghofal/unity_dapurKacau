using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> listKitchen;
    public event EventHandler<OnIngredientAddEventArgs> OnIngredientAdd;

    public class OnIngredientAddEventArgs: EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> listOfValidIngredients;

    private void Awake()
    {
        listKitchen = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // jika RecipeListValid tidak memimiliki KitchenObjectSO maka return false
        if (!listOfValidIngredients.Contains(kitchenObjectSO))
        {
            return false;
        }
        // jika listkitchen sudah memiliki kitchenObjectSO maka return false
        if (listKitchen.Contains(kitchenObjectSO))
        {
            return false;
        }
        // jika belum maka tambahkan kitchenObjectSO ke listKitchen lalu return true
        else
        {
            listKitchen.Add(kitchenObjectSO);
            OnIngredientAdd?.Invoke(this, new OnIngredientAddEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }


        
    }
}
