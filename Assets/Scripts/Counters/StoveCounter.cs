using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    private FryingRecipeSO fryingRecipeSO;
    private float fryingTimer;
    private State state;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                if (HasKitchenObject())
                {
                    fryingTimer += Time.deltaTime;
                    if (fryingTimer > fryingRecipeSO.timeCook)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                    }
                }
                break;
            case State.Fried:
                break;
            case State.Burned:
                break;
        }
        Debug.Log(state);

        
    }

    public override void Interact(Pemain pemain)
    {
        if (!HasKitchenObject())
        {
            // dan jika pemain memiliki kitchen object, maka kitchen object pada pemain di pindahkan ke clear counter
            if (pemain.HasKitchenObject())
            {
                if (HasRecipe(pemain.GetKitchenObject().GetKitchenObjectSO()))
                {
                    
                    pemain.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                    fryingTimer = 0;
                    state = State.Frying;


                }
            }
            // dan jika pemain tidak memiliki kitchen object 
            else
            {

            }
        }
        // if there is kitchen object on a clear counter
        else
        {
            // and if pemain has kitchen object
            if (pemain.HasKitchenObject())
            {

            }
            // and if pemain did not have the kitchen object, kitchen object on this clear counter move to the pemain
            else
            {
                GetKitchenObject().SetKitchenObjectParent(pemain);
            }
        }
    }

    private KitchenObjectSO GetFryItemRecipeFromSO(KitchenObjectSO kitchen)
    {
        FryingRecipeSO fryRecipeSO = GetFryRecipeSO(kitchen);
        if (fryRecipeSO.input == kitchen)
        {
            return fryRecipeSO.output;
        }
        return null;

    }

    private bool HasRecipe(KitchenObjectSO kitchen)
    {
        FryingRecipeSO fryRecipeSO = GetFryRecipeSO(kitchen);
        if (fryRecipeSO.input == kitchen)
        {
            return true;
        }
        return false;
    }

    private FryingRecipeSO GetFryRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeAny = null;
        foreach (FryingRecipeSO fryRecipeSO in fryingRecipeSOArray)
        {
            fryingRecipeAny = fryRecipeSO;
            if (fryRecipeSO.input == kitchenObjectSO)
            {
                return fryRecipeSO;
            }
        }
        return fryingRecipeAny;
        
    }
}
