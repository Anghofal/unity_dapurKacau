using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplate : MonoBehaviour
{
    // Getting GameObject type of Image
    [SerializeField] private Image image;

    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        // Set GameObject sprite equal to kitchenObjectSO.sprite
        image.sprite = kitchenObjectSO.sprite;
    }
}
