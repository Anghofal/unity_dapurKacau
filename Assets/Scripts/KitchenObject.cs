using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    // Gonna set spesific kitchenObjectSO to spesific kitchen object example kitchen object tomato gonna have
    // kitchenObjectSO of tomato
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Getting reference of the interface later each of counter must extend this interface
    // Later different counter that alredy extend interface can used all the function here 
    private IKitchenObjectParent kitchenObjectParent;

    // Return spesific kitchenObjectSO of this GameObject
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    // kitchenObjectParent can be ClearCounter ContainerCounter pemain or any counter that alredy extend the interface
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // If this.kitchenObjectParent ( example this.ClearCounter ) is not null
        // Or if there is something on this.kitchenObjectParent ( example this.ClearCounter )
        if (this.kitchenObjectParent != null)
        {
            // Call this.kitchenObjectParent.ClearKitchenObject ( example this.ClearCounter )
            // This function will set this.kitchenObjectParent ( example this.ClearCounter ) to null
            this.kitchenObjectParent.ClearKitchenObject();
        }
        // Move parent from this.kitchenObjectParent ( example this.ClearCounter ) to
        // kitchenObjectParent ( example kitchenObjectParent is pemain ) 
        // Now the kitchenObject knows the parent is pemain ( pick up items logic )
        this.kitchenObjectParent = kitchenObjectParent;

        // For example if the kitchen object is tomato next we make sure the pemain knows he is holding tomato
        // Set the kitchen object from base counter script to spesific kitchen object ( example tomato )
        kitchenObjectParent.SetKitchenObject(this);

        // We spawn the kitchen object to spesific location in this case to pemain
        transform.parent = kitchenObjectParent.GetKitchenObjectLocation();
        transform.localPosition = Vector3.zero;
    }

    // A function that if we call will return parameter plateKitchenObject and a boolean
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        // Check if this GameObject has class or script of PlateKitchenObject
        if (this is PlateKitchenObject)
        {
            // Fill the plateKitchenObject with this kichen object ( example tomato )
            // Then convert that kitchen object to PlateKitchenObject
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    
    public void DestroySelf()
    {
        // Clear the parent of this kitchen object and then destroy the game object
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        // Get this kitchen object kitchenObjectSO and then spawned it
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        
        // Got the reference from game object that we just spawned
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        // Set parent of kitchen object just spawned to the parameter
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
