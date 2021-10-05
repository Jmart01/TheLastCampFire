using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera DestinationCam;
    [SerializeField] float TransitionTime = 1.0f;
    CinemachineBrain cinemachineBrain;

    private void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            //moveCamera To a location
            cinemachineBrain.m_DefaultBlend.m_Time = TransitionTime;
            DestinationCam.Priority = 11;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Move Camera back to the player
        DestinationCam.Priority = 9;

    }
}
