using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    CameraMoveComp cameraMoveComp;

    private void Start()
    {
        cameraMoveComp = GetComponent<CameraMoveComp>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            //moveCamera To a location
            cameraMoveComp.TransitionCam();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Move Camera back to the player
        cameraMoveComp.ResetPriority();
    }
}
