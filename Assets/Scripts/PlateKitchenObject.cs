using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> listKitchen;

    private void Awake()
    {
        listKitchen = new List<KitchenObjectSO>();
    }

    public void addIngredient(KitchenObjectSO kitchenObjectSO)
    {
        listKitchen.Add(kitchenObjectSO);
    }
}
