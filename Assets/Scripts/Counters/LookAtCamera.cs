using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Creating enum so we can change the enum Mode later
    private enum Mode
    {
        LookAt,
        LookInverted,
        CameraForward,
        CameraForwardInverted,
    }

    // We can change mode from unity editor
    [SerializeField] private Mode mode;

    // This Update function will be called last after all the update
    private void LateUpdate()
    {
        
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform); 
                break;
            case Mode.LookInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(dirFromCamera + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
