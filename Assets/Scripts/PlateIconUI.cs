using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    // Got Reference from IconTemplate Game object
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        // Disable the IconTemplate 
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Getting the event from PlateKitchenObject
        plateKitchenObject.OnIngredientAdd += PlateKitchenObject_OnIngredientAdd;
    }

    private void PlateKitchenObject_OnIngredientAdd(object sender, PlateKitchenObject.OnIngredientAddEventArgs e)
    {
        UpdateVisualIcon();
    }

    private void UpdateVisualIcon()
    {
        // For each game object in this PlateIconUI
        foreach (Transform child in transform)
        {
            // If the object is iconTemplate skip the iteration
            if (child == iconTemplate) continue;
            Destroy(child);
        }
        // For each kitchenObjectSO in List
        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            // Instantiate iconTemplate to This GameObject
            Transform iconTransform = Instantiate(iconTemplate, transform);
            // Call function SetKitchenObjectSO from IconTemplate
            iconTransform.GetComponent<IconTemplate>().SetKitchenObjectSO(kitchenObjectSO);
            
        }
    }
}
