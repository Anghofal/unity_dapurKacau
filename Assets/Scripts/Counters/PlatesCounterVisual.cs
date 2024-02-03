using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform platePrefab;
    [SerializeField] private PlatesCounter platesCounter;
    private List<GameObject> platesList;

    private void Awake()
    {
        platesList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlatesSpawn += PlatesCounter_OnPlatesSpawn;
        platesCounter.OnPlatesRemove += PlatesCounter_OnPlatesRemove;
    }

    private void PlatesCounter_OnPlatesRemove(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = platesList[platesList.Count - 1];
        platesList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlatesSpawn(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform =  Instantiate(platePrefab, counterTopPoint);

        float platePlusY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, platePlusY * platesList.Count, 0);

        platesList.Add(plateVisualTransform.gameObject);
    }
}
