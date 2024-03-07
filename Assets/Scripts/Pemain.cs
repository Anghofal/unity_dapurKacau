using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pemain : MonoBehaviour, IKitchenObjectParent
{
    // Edit this when pemain is walking ( clicking movement button )
    private bool isWalking;

    // Edit this to change pemain movement speed from unity editor
    [SerializeField] private float movSpeed;
    
    // Reference script GameInput
    [SerializeField] private GameInput gameInput;

    // Reference layer mask we made for the counter
    [SerializeField] private LayerMask countersLayerMask;
    
    // Last move direction pemain is going to
    private Vector3 lastMoveDir;

    // Reference for the what counter pemain selected to example selectedCounter.ClearCounter
    BaseCounter selectedCounter;
    
    // Firing event when we select one of the counter
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    // Empty GameObject later we gonna spawn kitchen object to this point
    [SerializeField] private Transform kitchenObjectHoldPoint;

    // Reference for what kitchen object what pemain holding
    private KitchenObject kitchenObject;

    // Getting reference directly using instance so later we can use function of this script directly
    /*
    private static Pemain instanceField;
    public static Pemain GetInstanceField()
    {
        return instanceField;
    }
    private static void SetInstanceField(Pemain instanceField)
    {
        Pemain.instanceField = instanceField;
    }
    */
    
    // Getting reference directly using instance so later we can use function of this script
    // directly by accesing this instance ( example Pemain.Instance.GetKitchenObject )
    private static Pemain instance;
    public static Pemain Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    // compact version
    // public static Pemain Instance {get; private set;}

    // Return kitchenObjectHoldPoint ( point where the pemain is holding the kitchen object )
    public Transform GetKitchenObjectLocation()
    {
        return kitchenObjectHoldPoint;
    }

    // Set this.kitchenObject to kitchenObject we get
    // Basicly we set parent kitchen object to pemain
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    // To know what kitchenObject is player holding
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // To delete the reference of pemain kitchen object
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // To check if pemain holding a kitchen object
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    // Class to store variable when firing event
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    



    private void Start()
    {
        // If key button of E is pressed
        gameInput.OnInteractActions += GameInput_OnInteractActions;

        // If key button of F is pressed
        gameInput.OnAlternateInteract += GameInput_OnAlternateInteract;
    }

    private void GameInput_OnAlternateInteract(object sender, EventArgs e)
    {
        // If we near the counter
        if (selectedCounter != null)
        {
            // Call the function InteractAlternate on that counter ( example CuttingCounter.InteractAlternate )
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update()
    {
        // Call HandleMovement every update function
        HandleMovement();

        // Call HandleMovement every update function
        HandleInteractions();

    }

    private void Awake()
    {
        // Set the Instance to this class
        if (Instance != null)
        {
            Debug.LogError("Script Class Pemain telah memiliki Instance yang lain");
        }
        Instance = this;
    }
    private void GameInput_OnInteractActions(object sender, System.EventArgs e)
    {
        // If we near the counter
        if (selectedCounter != null)
        {
            // Call the function Interact on that counter ( example CuttingCounter.Interact )
            selectedCounter.Interact(this);
        }
    }

    public bool GetisWalking()
    {
        // Later this variable will be edited to know pemain is doing walking or not
        return isWalking;
    }

    private void HandleMovement() 
    {
        // Getting movement vector
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Fill Vector2 inputVector to Vector3 moveDir
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Save last move direction of the pemain
        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }

        // Creating variable to know how far pemain is go using movement speed
        // Multiply the movement speed by Time.deltaTime to get fix fps
        float moveDistance = movSpeed * Time.deltaTime;
        
        // Later used to add how far radius of CapsuleCast
        float playerRadius = 0.7f;

        // Edit this to edit how height of pemain is collider
        float playerHeight = 2f;


        // Creating capsule collider with detection
        // Point1 is the bottom of the capsule Point2 is the top of pemain
        // Check if the cast is hit other collider on movement direction
        bool colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        // If the cast is hitting some collider
        if (colliderHit)
        {

            Vector3 checkMoveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            // Check if the cast is hit other collider on x direction
            colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, checkMoveDirX, moveDistance);

            // If there is collider and player is moving
            if (!colliderHit && moveDir.x != 0)
            {
                // move direction will be ( X , 0 , 0 )
                moveDir = checkMoveDirX;
            }
            // If there is no collider on x direction
            else
            {

                Vector3 checkMoveDirZ = new Vector3(0,0, moveDir.z).normalized;
                // Check if the cast is hit other collider on z direction 
                colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, checkMoveDirZ, moveDistance);

                // If there is collider and player is moving
                if (!colliderHit && moveDir.z != 0)
                {
                    // move direction will be ( 0 , 0 , Z )
                    moveDir = checkMoveDirZ;
                }
                // If there is no collider on z direction
                else
                {
                    
                    // Check if the cast is hit other collider on movement direction
                    colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
                }
            }
        }


        // If the cast is not hitting any collider pemain will move
        // 
        if (!colliderHit)
        {
            transform.position += moveDir * moveDistance;
        }

        // If theres no input isWalking will equal false
        isWalking = moveDir != Vector3.zero;

        // Rotate the pemain Z axis equal to X and Y direction ( moveDir )
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    private void HandleInteractions()
    {
        // Fill inputVector with key input
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Make Vector2 inputVector into Vector3 moveDir
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        // Later this variable will used to determine how far the RayCast
        float interactDistance = 1f;

        // Check if theres collider of with layer of countersLayerMask
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            // After RayCast hit any collider if the GameObject have component of BaseCounter
            // Output reference of that GameObject
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // If this.selectedCounter is not equal to the reference of GameObject that just we get
                if (selectedCounter != baseCounter)
                {
                    // Using function SetSelectedCounter(baseCounter) from pemain
                    // To set this.selectedCounter to GameObject that just we get
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                // If the GameObject pemain select is alredy equal to the reference of GameObject that just we get
                // We set this.selectedCounter to null so later we call this function again
                SetSelectedCounter(null);


            }
        }
        // If theres no collider or theres a collider but not the type of layer countersLayerMask
        else
        {
            // We set this.selectedCounter to null so later we call this function again
            SetSelectedCounter(null);
        }
    }
    
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        // Set pemain.selectedCounter to the selectedCounter function parameter
        this.selectedCounter = selectedCounter;

        // Fire the event
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}


