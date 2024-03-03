using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pemain : MonoBehaviour, IKitchenObjectParent
{
    // Edit this when pemain is walking ( clicking movement button )
    private bool isWalking;

    // Edit this to change pemain movement speed from unity
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
        
        gameInput.OnInteractActions += GameInput_OnInteractActions;
        gameInput.OnAlternateInteract += GameInput_OnAlternateInteract;
    }

    private void GameInput_OnAlternateInteract(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update()
    {

        HandleMovement();
        HandleInteractions();

    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Script Class Pemain telah memiliki Instance yang lain");
        }
        Instance = this;
    }
    private void GameInput_OnInteractActions(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    public bool GetisWalking()
    {
        return isWalking;
    }

    private void HandleMovement() 
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Perlu mendapatkan movement direction untuk mengedit transform.position berdasarkan
        // arah vector
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);


        // Gunakan Time.deltatime untuk variabel yang memiliki hubungan erat dengan fps
        // Seperti jarak tempuh dan kecepatan
        float moveDistance = movSpeed * Time.deltaTime;
        // Seberapa Jarak Antar Collider tinggi nilainya semakin jauh jarak antar collision atau
        // batasnya
        float playerRadius = 0.7f;
        // Tinggi collider nya
        float playerHeight = 2f;
        // Membuat Kapsul collider dan deteksinya
        // point1 adalah titik bawah kapsul, point2 adalah titik atas kapsul
        bool colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        // Jika menabrak collider atau tembok
        if (colliderHit)
        {

            Vector3 checkMoveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            // Lakukan pengecekan apakah pada arah vector x masih ada collider
            colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, checkMoveDirX, moveDistance);


            if (!colliderHit && moveDir.x != 0)
            {
                // Jika tidak ada maka movement direction player akan bernilai (X, 0, 0)
                moveDir = checkMoveDirX;
            }
            else
            {

                Vector3 checkMoveDirZ = new Vector3(0,0, moveDir.z).normalized;
                // Lakukan pengecekan apakah pada arah vector z masih ada collider
                colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, checkMoveDirZ, moveDistance);

                if (!colliderHit && moveDir.z != 0)
                {
                    // Jika tidak ada maka movement direction player akan bernilai (0, 0, Z)
                    moveDir = checkMoveDirZ;
                }
                else
                {
                    colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
                }
            }
        }



        if (!colliderHit)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    private void HandleInteractions()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Perlu mendapatkan movement direction untuk mengedit transform.position berdasarkan
        // arah vector
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        float interactDistance = 1f;

        // Cek apakah terjadi collider collision lagi menggunakan metode raycast
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            // Sama seperti
            // ClearCounter clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
            // if (clearCounter != null){}
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(selectedCounter != baseCounter)
                {
                    // setting selectedCounter (Counter global variabel that will we share later)
                    // from clearCounter we get from raycastHit
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                // setting selectedCounter (Counter global variabel to null)
                // to make sure clearCounter is always null
                // so the condition will always met and clearCounter will always set to
                // new clear counter
                SetSelectedCounter(null);


            }
        }
        else
        {
            SetSelectedCounter(null);
        }

        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }

    }
    
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}


