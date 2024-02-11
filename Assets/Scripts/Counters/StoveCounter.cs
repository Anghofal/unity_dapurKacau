using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static IHasProgressBarUI;

public class StoveCounter : BaseCounter, IHasProgressBarUI
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    public event EventHandler<OnstateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgressBarUI.OnProgressChangeEventArgs> OnProgressChange;

    public class OnstateChangedEventArgs: EventArgs
    {
        public State state;
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private float fryingTimer;
    private float burningTimer;
    private State state;
    private float progresNormalized;

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
                    progresNormalized = fryingTimer / fryingRecipeSO.timeCook;
                    OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
                    {
                        progresNormalized = progresNormalized,
                    });
                    if (fryingTimer > fryingRecipeSO.timeCook)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        burningRecipeSO = GetBurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Fried;
                        burningTimer = 0f;
                        
                    }
                }
                break;
            case State.Fried:
                if (HasKitchenObject())
                {
                    burningTimer += Time.deltaTime;
                    progresNormalized = burningTimer / burningRecipeSO.burningTime;
                    OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
                    {
                        progresNormalized = progresNormalized,
                    });
                    if (burningTimer > burningRecipeSO.burningTime)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                    }
                }
                break;
            case State.Burned:
                OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
                {
                    progresNormalized = 0f,
                });
                break;
        }
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
                    OnStateChanged?.Invoke(this, new OnstateChangedEventArgs
                    {
                        state = state
                    });


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
                // jika kitchen object yang di pegang pemain adalah piring
                if (pemain.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // dapatkan kitchen object yang dipegang pemain sebagai plateKitchenObject
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            // and if pemain did not have the kitchen object, kitchen object on this clear counter move to the pemain
            else
            {
                GetKitchenObject().SetKitchenObjectParent(pemain);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnstateChangedEventArgs
                {
                    state = state
                });
                OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
                {
                    progresNormalized = 0f,
                });
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

    private BurningRecipeSO GetBurningRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        BurningRecipeSO fryingRecipeAny = null;
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            fryingRecipeAny = burningRecipeSO;
            if (burningRecipeSO.input == kitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return fryingRecipeAny;
    }
}
