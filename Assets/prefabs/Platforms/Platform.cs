using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Togglable
{
    void ToggleOn();
    void ToggleOff();
}


public class Platform : MonoBehaviour, Togglable
{
    [SerializeField] Transform ObjectToMove;
    [SerializeField] float transitionTime;

    Coroutine MovingCoroutine;

    public Transform StartTrans;
    public Transform EndTrans;

    public void ToggleOn()
    {
        MoveTo(true);
    }

    public void ToggleOff()
    {
        MoveTo(false);
    }

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
        Vector3 originPos = ObjectToMove.position;
        Quaternion originRot = ObjectToMove.rotation;
        while(timer < TransitionTime)
        {
            timer += Time.deltaTime;
            ObjectToMove.position = Vector3.Lerp(originPos, Destination.position, timer/TransitionTime);
            ObjectToMove.rotation = Quaternion.Lerp(originRot, Destination.rotation, timer / TransitionTime);
            yield return new WaitForEndOfFrame();
        }
        MovingCoroutine = null;
    }
    public Transform GetObjectToMove() { return ObjectToMove; }
    public Coroutine GetCoroutine() { return MovingCoroutine; }
}
