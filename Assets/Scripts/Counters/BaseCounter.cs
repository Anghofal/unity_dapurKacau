using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform pointTopOfClearCounter;
    private KitchenObject kitchenObject;

    // sebuah abstract function yang dapat di ovveride setiap counter dengan sifat yang berbeda - beda
    public virtual void Interact(Pemain pemain)
    {
        Debug.LogError("The function must be overrided");
    }

    public virtual void InteractAlternate(Pemain pemain)
    {
    }

    public Transform GetKitchenObjectLocation()
    {
        return pointTopOfClearCounter;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

}
