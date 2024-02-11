using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlatesSpawn;
    public event EventHandler OnPlatesRemove;

    [SerializeField] private KitchenObjectSO platesKitchenObjectSO;
    private float spawnTimer;
    private float spawnTimerMax = 4f;
    private float platesAlredySpawned;
    private float platesMaxSpawn = 4f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer > spawnTimerMax) {
            spawnTimer = 0f;
            
            if (platesAlredySpawned < platesMaxSpawn)
            {
                platesAlredySpawned++;
                OnPlatesSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Pemain pemain)
    {
        if (!pemain.HasKitchenObject())
        {
            // jika pemain tidak memiliki kitchenObject
            if (platesAlredySpawned > 0)
            {
                platesAlredySpawned--;
                KitchenObject.SpawnKitchenObject(platesKitchenObjectSO, pemain);
                OnPlatesRemove?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
