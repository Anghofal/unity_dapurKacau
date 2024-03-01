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
        // This function will set this.kitchenObjectParent ( example this.ClearCounter ) to
        // kitchenObjectParent from the parameter ( example kitchenObjectParent is pemain)
        // Basically pemain will set kitchenObjectParent to ClearCounter
        this.kitchenObjectParent = kitchenObjectParent;


        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("kitchen counter Alredy Has A Kitchen Object");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectLocation();
        transform.localPosition = Vector3.zero;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject=null;
            return false;
        }
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        
        // got the reference from game object that we just spawned
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
