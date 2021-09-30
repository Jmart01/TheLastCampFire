using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] Platform platformToMove;
    public void OnArtifactLeft()
    {
        Debug.Log("Artifact left, she took the kids");
        platformToMove.MoveTo(platformToMove.StartTrans);
    }

    public void OnArtifactPlaced()
    {
        platformToMove.MoveTo(platformToMove.EndTrans);
        Debug.Log("Artifact Placed");
    }
    public Transform GetSlotTrans()
    {
        return ArtifactSlotTrans;
    }
}
