using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerPathfinding : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    private InputAction moveAction;

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            var inputActions = playerInput.actions;
            if (inputActions != null)
            {
                var gameplayActionMap = inputActions.FindActionMap("Gameplay");
                if (gameplayActionMap != null)
                {
                    moveAction = gameplayActionMap.FindAction("Move");
                }
                else
                {
                    Debug.LogError("Gameplay action map not found.");
                }
            }
            else
            {
                Debug.LogError("InputActionAsset not assigned.");
            }
        }
        else
        {
            Debug.LogError("PlayerInput component not found.");
        }
    }

    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.Enable();
            moveAction.performed += OnMovePerformed;
        }
    }

    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.Disable();
            moveAction.performed -= OnMovePerformed;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (cam == null || agent == null)
        {
            Debug.LogError("Camera or NavMeshAgent not assigned.");
            return;
        }

        Debug.Log("Move action performed.");
        if (!IsPointerOverUIElement())
        {
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Debug.Log($"Mouse position: {screenPosition}");

            Ray ray = cam.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Raycast hit: {hit.point}");
                agent.SetDestination(hit.point);
            }
            else
            {
                Debug.Log("Raycast did not hit any objects.");
            }
        }
        else
        {
            Debug.Log("Pointer is over a UI element, not moving.");
        }
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}