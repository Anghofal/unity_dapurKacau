using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> listKitchen;

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
            return true;
        }


        
    }
}
