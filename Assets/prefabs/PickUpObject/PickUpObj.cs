using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : Interactable
{
    //[SerializeField] Transform newParentTransform;
    //[SerializeField] GameObject Player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void PickedUpBy(GameObject PickerGameObject)
    {
        Transform pickUpSocketTransform = PickerGameObject.transform;

        Player PickerAsPlayer = PickerGameObject.GetComponent<Player>();
        if(PickerAsPlayer != null)
        {
            pickUpSocketTransform = PickerAsPlayer.GetPickupSocketTransform();
        }

        transform.rotation = pickUpSocketTransform.rotation;
        transform.parent = pickUpSocketTransform;
        transform.localPosition = Vector3.zero;
    }
    public virtual void DropItem()
    {
        gameObject.transform.parent = null;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
    public override void Interact(GameObject InteractingGameObject)
    {
        //this works, just commented out to keep up with li's version
        /*Debug.Log($"I am interacting with: {}")
        float Dot = Vector3.Dot((transform.position - Player.transform.position).normalized, Player.transform.forward);
        Debug.Log(Dot);
        if(Dot > 0)
        {
            gameObject.transform.position = newParentTransform.position;
            gameObject.transform.parent = newParentTransform;
        }*/
        if(transform.parent == null)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Vector3 DirFromInteractingGameObj = (transform.position - InteractingGameObject.transform.position).normalized;
            Vector3 DirOfInteractingGameObj = InteractingGameObject.transform.forward;
            float Dot = Vector3.Dot(DirOfInteractingGameObj, DirFromInteractingGameObj);
            if (Dot > .5f)
            {
                PickedUpBy(InteractingGameObject);
            }
        }
        else
        {
            DropItem();
        }
    }
}
