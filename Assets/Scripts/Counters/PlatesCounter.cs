using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlatesSpawn;
    private float spawnTimer;
    private float spawnTimerMax = 4f;
    private float platesAlredySpawned;
    private float platesMaxSpawn = 4f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        Debug.Log(spawnTimer);
        if (spawnTimer > spawnTimerMax) {
            spawnTimer = 0f;
            
            if (platesAlredySpawned < platesMaxSpawn)
            {
                platesAlredySpawned++;
                OnPlatesSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
