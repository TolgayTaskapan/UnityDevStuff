using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuManager : MonoBehaviour
{
    public GameObject contextMenu; // Reference to the context menu panel
    public Button[] buttons; // Array of buttons for different actions
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        contextMenu.SetActive(false); // Hide the context menu at start

        // Add listeners to the buttons (assuming there are 3 buttons)
        buttons[0].onClick.AddListener(() => OnExamine());
        buttons[1].onClick.AddListener(() => OnPickUp());
        buttons[2].onClick.AddListener(() => OnUse());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click detection
        {
            // Get the position of the mouse in the world
            Vector3 screenPosition = Input.mousePosition;
            contextMenu.transform.position = screenPosition;

            // Raycast to detect what object is being right-clicked
            Ray ray = cam.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Show the context menu at the mouse position
                contextMenu.SetActive(true);
                // Optionally, set the context for the actions, e.g., the clicked object
                // SetContext(hit.collider.gameObject);
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2)) // Left or middle click
        {
            // Hide the context menu on any other click
            contextMenu.SetActive(false);
        }
    }

    // Example action methods
    void OnExamine()
    {
        Debug.Log("Examine action");
        contextMenu.SetActive(false);
    }

    void OnPickUp()
    {
        Debug.Log("Pick Up action");
        contextMenu.SetActive(false);
    }

    void OnUse()
    {
        Debug.Log("Use action");
        contextMenu.SetActive(false);
    }

    // Optionally, a method to set context, like the object being interacted with
    // void SetContext(GameObject obj)
    // {
    //     // Store the object to interact with
    // }
}
