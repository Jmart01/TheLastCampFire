using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMoveComp : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera DestinationCam;
    [SerializeField] float TransitionTime = 1.0f;
    CinemachineBrain cinemachineBrain;


    private void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void TransitionCam()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = TransitionTime;
        DestinationCam.Priority = 11;
    }

    public void ResetPriority()
    {
        DestinationCam.Priority = 9;
    }
}
