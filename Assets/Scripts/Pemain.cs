using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pemain : MonoBehaviour
{
    //Ingat selalu gunakan private dan serializefield dan bukan public
    [SerializeField] private float movSpeed;
    ClearCounter selectedCounter;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    private Vector3 lastMoveDir;

    private bool isWalking;



    private void Start()
    {
        gameInput.onInteractActions += GameInput_onInteractActions;
    }

    private void Update()
    {

        HandleMovement();
        HandleInteractions();

    }
    private void GameInput_onInteractActions(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact();
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


            if (!colliderHit)
            {
                // Jika tidak ada maka movement direction player akan bernilai (X, 0, 0)
                moveDir = checkMoveDirX;
            }
            else
            {

                Vector3 checkMoveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                // Lakukan pengecekan apakah pada arah vector z masih ada collider
                colliderHit = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, checkMoveDirZ, moveDistance);

                if (!colliderHit)
                {
                    // Jika tidak ada maka movement direction player akan bernilai (0, 0, Z)
                    moveDir = checkMoveDirZ;
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
        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }
        float interactDistance = 2f;

        // Cek apakah terjadi collider collision lagi menggunakan metode raycast
        if (Physics.Raycast(transform.position, moveDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            // Sama seperti
            // ClearCounter clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
            // if (clearCounter != null){}
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if(selectedCounter != clearCounter)
                {
                    selectedCounter = clearCounter;
                }
                else
                {
                    selectedCounter = null;
                }
            }
            else
            {
                selectedCounter = null;
            }
        }

    }
}


