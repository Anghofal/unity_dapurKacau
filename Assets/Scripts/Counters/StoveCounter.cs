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
                // If this GameObject Has a kitchen object
                if (HasKitchenObject())
                {
                    // burningTimer += Time Needed to call all last Update function
                    burningTimer += Time.deltaTime;

                    // Normalize the value to percentage
                    progresNormalized = burningTimer / burningRecipeSO.burningTime;

                    // Firing the event and sending progresNormalized
                    OnProgressChange?.Invoke(this, new IHasProgressBarUI.OnProgressChangeEventArgs
                    {
                        progresNormalized = progresNormalized,
                    });
                    // If fryingTimer is more than burningRecipeSO.timeCook or 5
                    if (burningTimer > burningRecipeSO.burningTime)
                    {
                        // Get This KitchenObject Destroy and call the Function from KitchenObject
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        // Change the State to State.Burned
                        state = State.Burned;
                    }
                }
                break;
                // If State is State.Burned
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
        // If this GameObject did not has a kitchen object
        if (!HasKitchenObject())
        {
            // And if pemain has a kitchen object
            if (pemain.HasKitchenObject())
            {
                // If HasRecipe(pemain.GetKitchenObject().GetKitchenObjectSO()) is True
                if (HasRecipe(pemain.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Get kitchen object pemain is holding and set parent to this GameObject
                    pemain.GetKitchenObject().SetKitchenObjectParent(this);

                    // Fill fryingRecipeSO from fryingRecipeSOList
                    fryingRecipeSO = GetFryRecipeSO(GetKitchenObject().GetKitchenObjectSO());

                    // Set fryingTimer and State to State.Frying
                    fryingTimer = 0;
                    state = State.Frying;

                    // Firing Event And Changing State
                    OnStateChanged?.Invoke(this, new OnstateChangedEventArgs
                    {
                        state = state
                    });


                }
            }
            // And if pemain did'nt have kitchen object
            else
            {

            }
        }
        // if there is kitchen object on a Stove counter ( Take kitchen object to plate )
        else
        {
            // and if pemain has kitchen object
            if (pemain.HasKitchenObject())
            {
                // If pemain.GetKitchenObject()TryGetPlate is True output reference plateKitchenObject
                if (pemain.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // If plateKitchenObject.TryAddIngredient Is True and add ingredient to List
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Destroy kitchen object on this counter 
                        GetKitchenObject().DestroySelf();
                        
                        // Change State to idle
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
            // And if pemain did not have the kitchen object ( Take kitchen object to pemain )
            else
            {
                // Set Kitchen object parent from this counter to pemain
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

    
    private bool HasRecipe(KitchenObjectSO kitchenObjectSO)
    {
        // Getting fryRecipeSO from GetFryRecipeSO(kitchen) function
        FryingRecipeSO fryRecipeSO = GetFryRecipeSO(kitchenObjectSO);

        // If kitchenObjectSO is equal to fryRecipeSO.input
        if (fryRecipeSO.input == kitchenObjectSO)
        {
            return true;
        }
        return false;
    }

    private FryingRecipeSO GetFryRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        // Creating empty variable to store variable type FryingRecipeSO
        FryingRecipeSO fryingRecipeAny = null;

        // For each fryRecipeSO in array
        foreach (FryingRecipeSO fryRecipeSO in fryingRecipeSOArray)
        {
            // Fill the fryingRecipeAny per iteration with fryRecipeSO
            fryingRecipeAny = fryRecipeSO;

            // If kitchenObjectSO is equal to the frying recipe ( MeatPatty Uncooked )
            if (fryRecipeSO.input == kitchenObjectSO)
            {
                // Return that kitchenObjectSO
                return fryRecipeSO;
            }
        }
        // If not return the last iteration Of The Recipe
        return fryingRecipeAny;
        
    }

    private BurningRecipeSO GetBurningRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        // Creating empty variable to store variable type BurningRecipeSO
        BurningRecipeSO burningRecipeAny = null;

        // For each burningRecipeSO in array
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            // Fill the fryingRecipeAny per iteration with burningRecipeSO
            burningRecipeAny = burningRecipeSO;

            // If kitchenObjectSO is equal to the burning recipe ( example MeatPatty Cooked == MeatPatty Cooked )
            if (burningRecipeSO.input == kitchenObjectSO)
            {
                // Return that kitchenObjectSO
                return burningRecipeSO;
            }
        }
        return burningRecipeAny;
    }
}
