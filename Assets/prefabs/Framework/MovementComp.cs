using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComp : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float GroundCheckRadius = 0.1f;
    [SerializeField] Transform RayCastOrigin;


    [Header("Ground Check")]
    [SerializeField] LayerMask GroundLayerMask;
    CharacterController characterController;
    
    Vector2 MoveInput;
    Vector3 velocity;
    float Gravity = -9.8f;
    
    bool isClimbing;
    Vector3 LadderDir;
    
    [SerializeField] Transform GroundCheck;
    Transform currentFloor;
    Vector3 PreviousWorldPos;
    Vector3 PreviousFloorLocalPos;
    Quaternion PreviousWorldRotation;
    Quaternion PreviousFloorLocalRotation;

    public void SetMovementInput(Vector2 inputVal)
    {
        MoveInput = inputVal;
    }
    public void ClearVerticalVelocity()
    {
        velocity.y = 0;
    }
    public void SetClimbingInfo(Vector3 ladderDir, bool climbing)
    {
        LadderDir = ladderDir;
        isClimbing = climbing;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void CheckFloor()
    {
        Collider[] cols = Physics.OverlapSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
        if (cols.Length != 0)
        {
            if (currentFloor != cols[0].transform)
            {
                currentFloor = cols[0].transform;
                SnapShotPositionAndRotation();
            }
        }
    }

    void SnapShotPositionAndRotation()
    {
        PreviousWorldPos = transform.position;
        PreviousWorldRotation = transform.rotation;

        if (currentFloor != null)
        {
            PreviousFloorLocalPos = currentFloor.InverseTransformPoint(transform.position);
            PreviousFloorLocalRotation = Quaternion.Inverse(currentFloor.rotation) * transform.rotation;
            //to add 2 rotations you do quaternionA * QuaternionB
            //to subtract you do quaternion.Inverse(QuaternionA) * QuaternionB
        }
    }

    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
    }

    public IEnumerator MoveToTransform(Transform Destination, float transformTime)
    {
        Vector3 StartPos = transform.position;
        Vector3 EndPos = Destination.position;
        Quaternion StartRot = transform.rotation;
        Quaternion EndRot = Destination.rotation;

        float timer = 0f;
        while (timer < transformTime)
        {
            timer += Time.deltaTime;
            //move in here
            Vector3 DeltaMove = Vector3.Lerp(StartPos, EndPos, timer / transformTime) - transform.position;
            characterController.Move(DeltaMove);
            // Rot here
            transform.rotation = Quaternion.Lerp(StartRot, EndRot, timer / transformTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Update()
    {
        if (isClimbing)
        {
            CalculateClimbingVelocity();
        }
        else
        {
            CalculateWalkingVelocity();
        }
        SnapShotPositionAndRotation();
    }

    void followFloor()
    {
        if (currentFloor)
        {
            Vector3 DeltaMove = currentFloor.TransformPoint(PreviousFloorLocalPos) - PreviousWorldPos;
            velocity += DeltaMove / Time.deltaTime;

            Quaternion DestinationRot = currentFloor.rotation * PreviousFloorLocalRotation;
            Quaternion DeltaRot = Quaternion.Inverse(PreviousWorldRotation) * DestinationRot;

            transform.rotation = transform.rotation * DeltaRot;
        }
    }

    void CalculateClimbingVelocity()
    {
        velocity = Vector3.zero;
        if (MoveInput.magnitude == 0)
        {
            velocity = Vector3.zero;
            return;
        }
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDirection();

        float Dot = Vector3.Dot(LadderDir, PlayerDesiredMoveDir);
        velocity = Vector3.zero;
        if (Dot > 0)
        {

            velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
            velocity.y = walkingSpeed;
        }
        else
        {
            if (IsOnGround())
            {
                velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
            }
            velocity.y = -walkingSpeed;
        }
        characterController.Move((velocity) * Time.deltaTime);
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
            CheckFloor();
            followFloor();
            characterController.Move((velocity) * Time.deltaTime);
        }
    }


    public Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }

    void UpdateRotation()
    {
        if (isClimbing)
        {
            return;
        }
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDirection();
        if (PlayerDesiredDir.magnitude == 0)
        {
            PlayerDesiredDir = transform.forward;
        }

        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotation, Time.deltaTime * rotationSpeed);
    }
}
