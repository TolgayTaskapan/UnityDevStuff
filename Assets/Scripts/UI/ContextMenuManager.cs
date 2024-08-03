using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ContextMenuManager : MonoBehaviour
{
    public GameObject contextMenu; // Reference to the context menu panel
    public Button optionPrefab; // Prefab for the button options
    private Camera cam;
    private GameObject currentTarget; // The object that was right-clicked
    private bool optionClicked = false; // Flag to track if an option was clicked

    private InputAction rightClickAction;
    private InputAction leftClickAction;

    void Start()
    {
        cam = Camera.main;
        contextMenu.SetActive(false); // Hide the context menu at start
        Debug.Log("ContextMenuManager initialized. Context menu hidden.");

        // Setup Input Actions
        rightClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/rightButton");
        leftClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");

        rightClickAction.performed += OnRightClick;
        leftClickAction.performed += OnLeftClickOrMiddleClick;

        rightClickAction.Enable();
        leftClickAction.Enable();
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        Vector3 screenPosition = Mouse.current.position.ReadValue();
        Debug.Log("Right-click detected at screen position: " + screenPosition);

        Ray ray = cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            currentTarget = hit.collider.gameObject;
            Debug.Log("Raycast hit: " + currentTarget.name);

            ShowContextMenu(screenPosition, currentTarget);
        }
        else
        {
            Debug.Log("Raycast did not hit any collider.");
        }
    }

    private void OnLeftClickOrMiddleClick(InputAction.CallbackContext context)
    {
        Debug.Log("Left or middle mouse button clicked.");
        if (!optionClicked)
        {
            Debug.Log("No option clicked. Hiding context menu.");
            contextMenu.SetActive(false);
        }
        optionClicked = false; // Reset the flag for the next frame
    }

    void ShowContextMenu(Vector3 position, GameObject target)
    {
        Debug.Log("Showing context menu for target: " + target.name);
        // Clear existing options
        foreach (Transform child in contextMenu.transform)
        {
            Destroy(child.gameObject);
            Debug.Log("Destroyed previous context menu option.");
        }

        // Check if the object implements IInteractable
        IInteractable interactable = target.GetComponent<IInteractable>();

        if (interactable != null)
        {
            Debug.Log("IInteractable implementation found on target: " + target.name);
            List<string> actions = interactable.GetActions();

            float optionHeight = optionPrefab.GetComponent<RectTransform>().sizeDelta.y;
            float spacing = 5f; // Space between options
            Vector3 startPosition = new Vector3(position.x, position.y - optionHeight / 2, position.z);

            for (int i = 0; i < actions.Count; i++)
            {
                string action = actions[i];
                Button optionButton = Instantiate(optionPrefab, contextMenu.transform);

                // Set the action text on the button's child TextMeshProUGUI component
                TextMeshProUGUI buttonText = optionButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = action;
                }

                // Position each option with spacing
                RectTransform optionTransform = optionButton.GetComponent<RectTransform>();
                optionTransform.anchoredPosition = new Vector2(0, -i * (optionHeight + spacing));
                Debug.Log("Created option: " + action);

                // Set up the handler for clicks
                OptionHandler handler = optionButton.gameObject.AddComponent<OptionHandler>();
                handler.Setup(action, this);
            }

            // Adjust context menu size and show it
            RectTransform contextMenuTransform = contextMenu.GetComponent<RectTransform>();
            contextMenuTransform.sizeDelta = new Vector2(contextMenuTransform.sizeDelta.x, actions.Count * (optionHeight + spacing));
            contextMenu.SetActive(true);
            contextMenu.transform.position = position;
            Debug.Log("Context menu displayed with " + actions.Count + " options.");
        }
        else
        {
            Debug.LogError("No IInteractable implementation found on target: " + target.name);
        }
    }

    public void OnActionSelected(string action)
    {
        optionClicked = true; // Mark that an option was clicked
        Debug.Log("Action selected: " + action);

        if (currentTarget == null)
        {
            Debug.LogError("No current target.");
            return;
        }

        ItemInteractable itemInteractable = currentTarget.GetComponent<ItemInteractable>();
        if (itemInteractable != null)
        {
            Debug.Log("Performing action: " + action + " on target: " + currentTarget.name);
            itemInteractable.PerformAction(action);
        }
        else
        {
            Debug.LogError("ItemInteractable component not found on current target.");
        }

        contextMenu.SetActive(false);
        Debug.Log("Context menu hidden after action selection.");
    }

    private void OnDestroy()
    {
        // Disable actions when the object is destroyed
        rightClickAction.Disable();
        leftClickAction.Disable();
    }
}