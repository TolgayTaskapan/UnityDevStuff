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
        // Assume inputActions is assigned through the Inspector or script initialization
        InputActionAsset inputActions = GetComponent<PlayerInput>().actions;
        var gameplayActionMap = inputActions.FindActionMap("Gameplay");
        moveAction = gameplayActionMap.FindAction("Move");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        moveAction.performed += OnMovePerformed;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        moveAction.performed -= OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
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