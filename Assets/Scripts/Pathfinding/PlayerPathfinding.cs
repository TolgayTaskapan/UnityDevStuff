using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerPathfinding : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    public NavMeshAgent agent; // Reference to the NavMeshAgent

    [Header("Input Settings")]
    public InputActionAsset inputActions;
    private InputAction moveAction;

    private void Awake()
    {
        // Get the action map and action for player movement
        var gameplayActionMap = inputActions.FindActionMap("Gameplay"); // Ensure you have this action map defined
        moveAction = gameplayActionMap.FindAction("Move"); // Ensure you have this action defined
    }

    private void OnEnable()
    {
        moveAction.Enable();
        moveAction.performed += OnMove;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        moveAction.performed -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Check if the pointer is not over a UI element
        if (!IsPointerOverUIElement())
        {
            // Get the screen position from the pointer's position
            Vector2 screenPosition = context.ReadValue<Vector2>();

            // Create a ray from the camera to the screen position
            Ray ray = cam.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            // Perform a raycast to check if it hits any object
            if (Physics.Raycast(ray, out hit))
            {
                // Set the destination of the NavMeshAgent to the hit point
                agent.SetDestination(hit.point);
            }
        }
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
