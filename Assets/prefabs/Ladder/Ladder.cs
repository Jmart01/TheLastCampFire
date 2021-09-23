using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] Transform TopSnapTransform;
    [SerializeField] Transform BottomSnapTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player otherAsPlayer = other.GetComponent<Player>();
        if(otherAsPlayer!=null)
        {
            otherAsPlayer.NotifyLadderNearby(this);
        }
        //other.GetComponent<Transform>().position.y = gameObject.GetComponent<BoxCollider>().transform.position.y;
        //other.GetComponent<Player>().MoveOnLadder(GetComponent<BoxCollider>().center.y);
    }

    private void OnTriggerExit(Collider other)
    {
        Player otherAsPlayer = other.GetComponent<Player>();
        if (otherAsPlayer != null)
        {
            otherAsPlayer.NotifyLadderExit(this);
        }
    }
    public Transform GetClosestSnapTransform(Vector3 Position)
    {
        float DistanceToTop = Vector3.Distance(Position, TopSnapTransform.position);
        float DistanceToBot = Vector3.Distance(Position, BottomSnapTransform.position);
        return DistanceToTop < DistanceToBot ? TopSnapTransform : BottomSnapTransform;
    }
}
