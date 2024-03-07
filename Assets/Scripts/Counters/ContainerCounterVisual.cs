using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    // Reference for the containerCounter so we can listen the event
    [SerializeField] private ContainerCounter containerCounter;
    
    // Getting animator class so we can access the animation
    private Animator animator;

    // Same name as variable in ContainerCounter animation script
    private const string OPEN_CLOSE_ANIMATOR_VAR = "OpenClose";
    private void Awake()
    {
        // Get component from
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
