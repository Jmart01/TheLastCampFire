using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Transform ObjectToMove;
    [SerializeField] float transitionTime;

    Coroutine MovingCoroutine;

    public Transform StartTrans;
    public Transform EndTrans;

    public void MoveTo(bool ToEnd)
    {
        if(ToEnd)
        {
            MoveTo(EndTrans);
        }
        else
        {
            MoveTo(StartTrans);
        }
    }
   public void MoveTo(Transform Destination)
    {
        if(MovingCoroutine != null)
        {
            StopCoroutine(MovingCoroutine);
            MovingCoroutine = null;
        }
        MovingCoroutine = StartCoroutine(MoveToTrans(Destination, transitionTime));
    }

    IEnumerator MoveToTrans(Transform Destination, float TransitionTime)
    {
        float timer = 0f;
        while(timer < TransitionTime)
        {
            timer += Time.deltaTime;
            ObjectToMove.position = Vector3.Lerp(ObjectToMove.position, Destination.position, timer/TransitionTime);
            ObjectToMove.rotation = Quaternion.Lerp(ObjectToMove.rotation, Destination.rotation, timer / TransitionTime);
            yield return new WaitForEndOfFrame();
        }
        MovingCoroutine = null;
    }
    public Transform GetObjectToMove() { return ObjectToMove; }
    public Coroutine GetCoroutine() { return MovingCoroutine; }
}
