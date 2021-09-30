using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : PickUpObj
{
    [SerializeField] float DropdownSlotSearchRadius = .2f;
    ArtifactSlot CurrentSlot = null;
    private void Start()
    {
        DropItem();
    }
    public override void PickedUpBy(GameObject PickerGameObject)
    {
        base.PickedUpBy(PickerGameObject);
        if(CurrentSlot)
        {
            CurrentSlot.OnArtifactLeft();
        }
    }
    public override void DropItem()
    {
        ArtifactSlot slot = GetArtifactSlotNearby();
        if(slot != null)
        {
            slot.OnArtifactPlaced();
            transform.parent = null;
            transform.rotation = slot.GetSlotTrans().rotation;
            transform.position = slot.GetSlotTrans().position;
            CurrentSlot = slot;
        }
        else
        {
            base.DropItem();
        }
    }
    ArtifactSlot GetArtifactSlotNearby()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, DropdownSlotSearchRadius);
        foreach(Collider col in Cols)
        {
            ArtifactSlot slot = col.GetComponent<ArtifactSlot>();
            if(slot !=null)
            {
                return slot;
            }
        }
        return null;
    }
}
