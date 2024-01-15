using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private const string OPEN_CLOSE_ANIMATOR_VAR = "OpenClose";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabKitchenObject += ContainerCounter_OnPlayerGrabKitchenObject;
    }

    private void ContainerCounter_OnPlayerGrabKitchenObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE_ANIMATOR_VAR);
    }
}
