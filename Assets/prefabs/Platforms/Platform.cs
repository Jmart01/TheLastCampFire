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
    PlatformMovementComp platformMovementComp;

    private void Start()
    {
        platformMovementComp = GetComponent<PlatformMovementComp>();
    }
    public void ToggleOn()
    {
        platformMovementComp.MoveTo(true);
    }

    public void ToggleOff()
    {
        platformMovementComp.MoveTo(false);
    }

    
}
