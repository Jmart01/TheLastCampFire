using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 5f;
    PlayerInputs inputActions;
    CharacterController characterController;
    Vector2 MoveInput;
    Vector3 velocity;

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
        velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }
}
