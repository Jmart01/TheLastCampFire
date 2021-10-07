using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderClimbingComp : MonoBehaviour
{
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    [SerializeField] float LadderHopOnTime = 0.2f;

    List<Ladder> LaddersNearby = new List<Ladder>();
    Ladder CurrentClimbingLadder;

    MovementComp movementComp;
    IInputActionCollection InputAction;
    public void SetInput(IInputActionCollection inputAction)
    {
        InputAction = inputAction;
    }

    private void Start()
    {
        movementComp = GetComponent<MovementComp>();
    }
    public void NotifyLadderNearby(Ladder ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }

    public void NotifyLadderExit(Ladder ladderExit)
    {
        if (ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            movementComp.SetClimbingInfo(Vector3.zero, false);
            movementComp.ClearVerticalVelocity();
        }
        LaddersNearby.Remove(ladderExit);
    }
    Ladder FindPlayerClimbingLadder()
    {
        Vector3 PlayerDesiredMoveDir = movementComp.GetPlayerDesiredMoveDirection();
        Ladder ChosenLadder = null;
        float ClosestAngle = 180;
        foreach (Ladder ladder in LaddersNearby)
        {
            Vector3 LadderDir = ladder.transform.position - transform.position;
            LadderDir.y = 0;
            LadderDir.Normalize();
            float Dot = Vector3.Dot(PlayerDesiredMoveDir, LadderDir);
            float AngleInDegrees = Mathf.Acos(Dot) * Mathf.Rad2Deg;
            if (AngleInDegrees < LadderClimbCommitAngleDegrees && AngleInDegrees < ClosestAngle)
            {
                ChosenLadder = ladder;
                ClosestAngle = AngleInDegrees;
            }
        }
        return ChosenLadder;
    }
    void HopOnLadder(Ladder ladderToHopOn)
    {
        if (ladderToHopOn == null) return;

        if (ladderToHopOn != CurrentClimbingLadder)
        {
            Transform snapToTransform = ladderToHopOn.GetClosestSnapTransform(transform.position);
            CurrentClimbingLadder = ladderToHopOn;
            movementComp.SetClimbingInfo(ladderToHopOn.transform.forward, true);
            DisableMovementInput();
            StartCoroutine(movementComp.MoveToTransform(snapToTransform, LadderHopOnTime));
            Invoke("EnableMovementInput", LadderHopOnTime);
        }
    }
    void EnableMovementInput()
    {
        InputAction.Enable();
    }

    void DisableMovementInput()
    {
        InputAction.Disable();
    }

    private void Update()
    {
        if (CurrentClimbingLadder == null)
        {
            HopOnLadder(FindPlayerClimbingLadder());
        }
    }
    public Ladder GetCurrentClimbingLadder() { return CurrentClimbingLadder; }
}
