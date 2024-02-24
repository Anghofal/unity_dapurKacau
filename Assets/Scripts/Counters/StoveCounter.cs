using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static IHasProgressBarUI;

public class StoveCounter : BaseCounter, IHasProgressBarUI
{
    // Creating enum with name of State
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    // Creating Event to sent enum State changes
    public event EventHandler<OnstateChangedEventArgs> OnStateChanged;
    // Creating Event to sent Time
    public event EventHandler<IHasProgressBarUI.OnProgressChangeEventArgs> OnProgressChange;
    public class OnstateChangedEventArgs: EventArgs
    {
        public State state;
    }

    // Creating Array that contain SO of Frying Recipe, Example : Fresh -> Cook
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    // Creating variable that next will store SO from fryingRecipeSOArr
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    // Time needed for frying
    private float fryingTimer;
    // Time needed for burned
    private float burningTimer;
    // Contain Normalize Number Of Something
    private float progresNormalized;
    // Contain what State this GameObject is now
    private State state;
    

    private void Start()
    {
        // To make sure every start state is on idle
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            // If State is State.Frying
            case State.Frying:
                // 
                if (HasKitchenObject())
                {
                    // fryingTimer += Time Needed to call all last Update function
                    fryingTimer += Time.deltaTime;
                    
                    // Normalize the value to percentage
                    progresNormalized = fryingTimer / fryingRecipeSO.timeCook;

                    // Firing the event and sending progresNormalized
                    OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
                    {
                        progresNormalized = progresNormalized,
                    });

                    // If fryingTimer is more than fryingRecipeSO.timeCook or 4
                    if (fryingTimer > fryingRecipeSO.timeCook)
                    {
                        // Get This KitchenObject Destroy and call the Function from KitchenObject
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        // Get burningRecipeSO from the burningRecipeSOArray
                        burningRecipeSO = GetBurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                        // Change the State to State.Fried and set the burningTimer to 0
                        state = State.Fried;
                        burningTimer = 0f;
                        
                    }
                }
                break;
            // If State is State.Fried
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
