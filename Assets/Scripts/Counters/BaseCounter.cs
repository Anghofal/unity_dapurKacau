using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    // This reference to empty GameObject of pointTopOfClearCounter
    [SerializeField] private Transform pointTopOfClearCounter;

    // This will be used to set or get what kitchen object is currently on this counter
    private KitchenObject kitchenObject;
    
    // Abstract function that can be override for counter GameObject
    public virtual void Interact(Pemain pemain)
    {
        
    }

    // Abstract function that can be override for counter GameObject
    public virtual void InteractAlternate(Pemain pemain)
    {
    }

    // Return this counter pointTopOfClearCounter
    public Transform GetKitchenObjectLocation()
    {
        return pointTopOfClearCounter;
    }

    // Set reference of some spesific kitchen object to this Counter
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    // What kitchen object on this counter
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // Set kitchen object on this counter back to null
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // If kitchen object on this counter is not null
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

}
