using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    // List to store what ingredient is added to the plate
    private List<KitchenObjectSO> listKitchen;

    // Event to know what ingredient is added to the plate
    public event EventHandler<OnIngredientAddEventArgs> OnIngredientAdd;
    public class OnIngredientAddEventArgs: EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    // List of what ingredient can be added to the plate
    [SerializeField] private List<KitchenObjectSO> listOfValidIngredients;

    private void Awake()
    {
        // Initialize the List
        listKitchen = new List<KitchenObjectSO>();
    }

    // Return the List of what ingredient can be added to the plate
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return listOfValidIngredients;
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // If kitchenObjectSO is not in the RecipeListValid
        if (!listOfValidIngredients.Contains(kitchenObjectSO))
        {
            // Return false
            return false;
        }
        // If kitchenObjectSO is alredy on the plate ( or in the listkitchen )
        if (listKitchen.Contains(kitchenObjectSO))
        {
            // Return false
            return false;
        }
        // If kitchenObjectSO is not on the the plate ( or in the listkitchen )
        else
        {
            // Add kitchenObjectSO to list
            listKitchen.Add(kitchenObjectSO);

            // Fire the event and bring kitchenObjectSO
            OnIngredientAdd?.Invoke(this, new OnIngredientAddEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }


        
    }
}
