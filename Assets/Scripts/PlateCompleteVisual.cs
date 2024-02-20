using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjects;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdd += PlateKitchenObject_OnIngredientAdd;

        // untuk setiap objek yang berada di List kitchenObjectSO_GameObjects
        foreach (KitchenObjectSO_GameObject kitchenObjectSO in kitchenObjectSO_GameObjects)
        {
            // objek yang di tunjuk di aktifkan
            kitchenObjectSO.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdd(object sender, PlateKitchenObject.OnIngredientAddEventArgs e)
    {
        // untuk setiap objek yang berada di List kitchenObjectSO_GameObjects
        foreach (KitchenObjectSO_GameObject kitchenObjectSO in kitchenObjectSO_GameObjects)
        {
            // jika objek yang di tunjuk sama dengan kitchenObjectSo yang ada pada event
            if(kitchenObjectSO.kitchenObjectSO == e.kitchenObjectSO)
            {
                // objek yang di tunjuk di aktifkan
                kitchenObjectSO.gameObject.SetActive(true);
            }
        }
    }
}
