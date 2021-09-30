using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : Interactable
{
    [SerializeField] Transform newParentTransform;
    [SerializeField] GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        float Dot = Vector3.Dot((transform.position - Player.transform.position).normalized, Player.transform.forward);
        Debug.Log(Dot);
        if(Dot > 0)
        {
            gameObject.transform.position = newParentTransform.position;
            gameObject.transform.parent = newParentTransform;
        }
    }
}
