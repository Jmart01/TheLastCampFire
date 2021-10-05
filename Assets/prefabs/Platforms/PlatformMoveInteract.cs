using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveInteract : Interactable
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(GameObject InteractingObject)
    {
        /*
        Player InteractorAsPlayer = InteractingObject.GetComponent<Player>();
        Debug.Log(InteractorAsPlayer.name);
        if (InteractorAsPlayer != null)
        {
            StartCoroutine(WaitForCoroutine(InteractingObject));
            InteractorAsPlayer.transform.parent = platformToMove.GetObjectToMove();
            platformToMove.MoveTo(platformToMove.EndTrans);
            Debug.Log("This should work");
        }
        else
        {
            Debug.Log("something is wrong");
            return;
        }*/

        GetComponentInChildren<Platform>().MoveTo(true);
    }
    /*IEnumerator WaitForCoroutine(GameObject player)
    {
        while(platformToMove.GetCoroutine() != null)
        {
            player.GetComponent<CharacterController>().enabled = false;
            yield return new WaitForEndOfFrame();
        }
        player.GetComponent<CharacterController>().enabled = true;
    }*/
}
