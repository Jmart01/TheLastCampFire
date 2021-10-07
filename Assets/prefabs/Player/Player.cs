using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] Transform PickupSocketTransform;
    PlayerInputs inputActions;

    MovementComp movementComp;
    LadderClimbingComp climbingComp;

    public Transform GetPickupSocketTransform()
    {
        return PickupSocketTransform;
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

    void Start()
    {
        movementComp = GetComponent<MovementComp>();
        climbingComp = GetComponent<LadderClimbingComp>();
        climbingComp.SetInput(inputActions);
        inputActions.Gameplay.Move.performed += MoveInputUpdated;
        inputActions.Gameplay.Move.canceled += MoveInputUpdated;
        inputActions.Gameplay.Interact.performed += Interact;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        InteractComponent interactComp = GetComponentInChildren<InteractComponent>();
        if (interactComp != null)
        {
            interactComp.Interact();
        }
    }
    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        movementComp.SetMovementInput(ctx.ReadValue<Vector2>());
    } 
}