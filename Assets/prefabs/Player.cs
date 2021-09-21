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
    [SerializeField] LayerMask GroundLayerMask;
    PlayerInputs inputActions;
    CharacterController characterController;
    Vector2 MoveInput;
    Vector3 velocity;
    float Gravity = -9.8f;

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
    }
    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOnGround())
        {
            velocity.y = -0.2f;
        }
        velocity.x = GetPlayerDesiredMoveDirection().x * walkingSpeed;
        velocity.z = GetPlayerDesiredMoveDirection().z * walkingSpeed;
        velocity.y += Gravity * Time.deltaTime;
        characterController.Move((velocity)* Time.deltaTime);
        UpdateRotation();
    }

    Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }

    void UpdateRotation()
    {
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDirection();
        if(PlayerDesiredDir.magnitude == 0)
        {
            PlayerDesiredDir = transform.forward;
        }
        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotation, Time.deltaTime * rotationSpeed);
    }
}
