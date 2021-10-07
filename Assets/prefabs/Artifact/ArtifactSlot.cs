using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] GameObject TogglingObject;
    public void OnArtifactLeft()
    {
        //Debug.Log("Artifact left, she took the kids");
        //platformToMove.MoveTo(platformToMove.StartTrans);
        //changing so that the slot can be used for other things
        TogglingObject.GetComponent<Togglable>().ToggleOff();
    }

    public void OnArtifactPlaced()
    {
        //platformToMove.MoveTo(platformToMove.EndTrans);
        //Debug.Log("Artifact Placed");
        TogglingObject.GetComponent<Togglable>().ToggleOn();
    }
    public Transform GetSlotTrans()
    {
        return ArtifactSlotTrans;
    }
}
