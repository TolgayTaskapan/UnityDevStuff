using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerPathfinding : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    public NavMeshAgent agent; // Reference to the NavMeshAgent

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            // Create a ray from the camera to the mouse position
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform a raycast to check if it hits any object
            if (Physics.Raycast(ray, out hit))
            {
                // Set the destination of the NavMeshAgent to the hit point
                agent.SetDestination(hit.point);
            }
        }
    }

    private bool IsPointerOverUIElement() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}

