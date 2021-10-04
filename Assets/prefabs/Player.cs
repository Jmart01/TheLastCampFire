using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] Transform GroundCheck;
    [SerializeField] float GroundCheckRadius = 0.1f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float LadderClimbCommitAngleDegrees= 20f;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] Transform RayCastOrigin;
    [SerializeField] Transform PickupSocketTransform;
    PlayerInputs inputActions;
    CharacterController characterController;
    Vector2 MoveInput;
    Vector3 velocity;
    float Gravity = -9.8f;
    public bool isOnLadder;

    List<Ladder> LaddersNearby = new List<Ladder>();
    Ladder CurrentClimbingLadder;

    public Transform GetPickupSocketTransform()
    {
        return PickupSocketTransform;
    }


    public void NotifyLadderNearby(Ladder ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }

    public void NotifyLadderExit(Ladder ladderExit)
    {
        if(ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            velocity.y = 0;
        }
        LaddersNearby.Remove(ladderExit);
    }

    Ladder FindPlayerClimbingLadder()
    {
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDirection();
        Ladder ChosenLadder = null;
        float ClosestAngle = 180;
        foreach(Ladder ladder in LaddersNearby)
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
    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
    }
    private void Awake()
    {
        inputActions = new PlayerInputs();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputActions.Gameplay.Move.performed += MoveInputUpdated;
        inputActions.Gameplay.Move.canceled += MoveInputUpdated;
        isOnLadder = false;
        inputActions.Gameplay.Interact.performed += Interact;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        InteractComponent interactComp = GetComponentInChildren<InteractComponent>();
        if(interactComp != null)
        {
            interactComp.Interact();
        }
    }
    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
    void HopOnLadder(Ladder ladderToHopOn)
    {
        if (ladderToHopOn == null) return;

        if(ladderToHopOn != CurrentClimbingLadder)
        {
            Transform snapToTransform = ladderToHopOn.GetClosestSnapTransform(transform.position);
            CurrentClimbingLadder = ladderToHopOn;
            StartCoroutine(MoveToTransform(snapToTransform, 0.2f));
        }
    }

    IEnumerator MoveToTransform(Transform Destination, float transformTime)
    {
        inputActions.Gameplay.Move.Disable();

        Vector3 StartPos = transform.position;
        Vector3 EndPos = Destination.position;
        Quaternion StartRot = transform.rotation;
        Quaternion EndRot = Destination.rotation;

        float timer = 0f;
        while(timer < transformTime)
        {
            timer += Time.deltaTime;
            //move in here
            Vector3 DeltaMove = Vector3.Lerp(StartPos, EndPos, timer / transformTime) - transform.position;
            characterController.Move(DeltaMove);
            // Rot here
            transform.rotation = Quaternion.Lerp(StartRot, EndRot, timer / transformTime);
            yield return new WaitForEndOfFrame();
        }
        inputActions.Gameplay.Move.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentClimbingLadder == null)
        {
            HopOnLadder(FindPlayerClimbingLadder());
        }
        if(CurrentClimbingLadder)
        {
            CalculateClimbingVelocity();
        }
        else
        {
            CalculateWalkingVelocity();
        }
    }

    void CalculateWalkingVelocity()
    {
        if (IsOnGround())
        {
            velocity.y = -0.2f;
        }
        velocity.x = GetPlayerDesiredMoveDirection().x * walkingSpeed;
        velocity.z = GetPlayerDesiredMoveDirection().z * walkingSpeed;
        velocity.y += Gravity * Time.deltaTime;
        UpdateRotation();
        RaycastHit hit;
        //Vector3 AverageBetweenForwardAndDownDir = (Vector3.forward + Vector3.down) / 2;
        if (Physics.Raycast(RayCastOrigin.position, RayCastOrigin.TransformDirection(Vector3.down), out hit, 3f, GroundLayerMask))
        {
            Debug.DrawRay(RayCastOrigin.position, RayCastOrigin.TransformDirection(Vector3.down) * hit.distance, Color.blue);
            characterController.Move((velocity) * Time.deltaTime);
        }
    }
    void CalculateClimbingVelocity()
    {
        velocity = Vector3.zero;
        if(MoveInput.magnitude == 0)
        {
            velocity = Vector3.zero;
            return;
        }
        Vector3 LadderDir = CurrentClimbingLadder.transform.forward;
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDirection();

        float Dot = Vector3.Dot(LadderDir, PlayerDesiredMoveDir);
        velocity = Vector3.zero;
        if(Dot > 0)
        {
            
            velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
            velocity.y = walkingSpeed;
        }
        else
        {
            if(IsOnGround())
            {
                velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
            }
            velocity.y = -walkingSpeed; 
        }
        characterController.Move((velocity) * Time.deltaTime);
    }

    Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }

    void UpdateRotation()
    {
        if(CurrentClimbingLadder != null)
        {
            return;
        }
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDirection();
        if(PlayerDesiredDir.magnitude == 0)
        {
            PlayerDesiredDir = transform.forward;
        }

        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotation, Time.deltaTime * rotationSpeed);
    }
}
