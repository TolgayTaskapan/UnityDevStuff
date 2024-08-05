using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContextMenuManager : MonoBehaviour
{
    public GameObject contextMenu; // Reference to the context menu panel
    public Button optionPrefab; // Prefab for the button options
    public List<GraphicRaycaster> graphicRaycasters; // List of GraphicRaycasters for multiple canvases
    public EventSystem eventSystem; // Reference to the EventSystem
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

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast to UI elements across multiple canvases
        foreach (var raycaster in graphicRaycasters)
        {
            List<RaycastResult> canvasResults = new List<RaycastResult>();
            raycaster.Raycast(pointerData, canvasResults);
            results.AddRange(canvasResults);
        }

        Debug.Log($"Number of UI elements hit: {results.Count}");
        foreach (var result in results)
        {
            Debug.Log($"Hit UI element: {result.gameObject.name}");
        }

        if (results.Count > 0)
        {
            Debug.Log("UI element clicked. Showing context menu for UI.");

            foreach (RaycastResult result in results)
            {

                GameObject clickedObject = result.gameObject;
                Debug.Log("Clicked object: " + clickedObject.ToString());
                Debug.Log("Object tag check...");
                if (clickedObject.CompareTag("Item"))
                {
                    Debug.Log("Object tagged Item, openning showing context menu...");
                    currentTarget = clickedObject;
                    ShowContextMenu(screenPosition, currentTarget, true);
                    return;
                }
            }
        }
        else
        {
            Ray ray = cam.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                currentTarget = hit.collider.gameObject;
                Debug.Log("Raycast hit: " + currentTarget.name);
                ShowContextMenu(screenPosition, currentTarget, false);
            }
            else
            {
                Debug.Log("Raycast did not hit any collider.");
            }
        }
    }

    private void OnLeftClickOrMiddleClick(InputAction.CallbackContext context)
    {
        Debug.Log("Left or middle mouse button clicked.");

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();

        foreach (var raycaster in graphicRaycasters)
        {
            List<RaycastResult> canvasResults = new List<RaycastResult>();
            raycaster.Raycast(pointerData, canvasResults);
            results.AddRange(canvasResults);
        }

        bool uiClicked = false;
        foreach (RaycastResult result in results)
        {
            Debug.Log($"Clicked on {result.gameObject.name}");

            OptionHandler handler = result.gameObject.GetComponent<OptionHandler>();
            if (handler != null)
            {
                handler.OnButtonClicked();
                uiClicked = true;
                break;
            }
        }

        if (!uiClicked)
        {
            Debug.Log("No option clicked. Hiding context menu.");
            contextMenu.SetActive(false);
        }

        optionClicked = uiClicked;
    }

    public void ShowContextMenu(Vector3 position, GameObject target, bool isInInventory)
    {
        Debug.Log("Showing context menu for target: " + target.name);
        foreach (Transform child in contextMenu.transform)
        {
            Destroy(child.gameObject);
            Debug.Log("Destroyed previous context menu option.");
        }

        IInteractable interactable = target.GetComponent<IInteractable>();

        if (interactable != null)
        {
            Debug.Log("IInteractable implementation found on target: " + target.name);
            List<string> actions = interactable.GetActions();

            if (isInInventory)
            {
                actions.Remove("Pick Up");
            }
            else
            {
                actions.Remove("Drop");
            }

            float optionHeight = optionPrefab.GetComponent<RectTransform>().sizeDelta.y;
            float spacing = 5f;
            Vector3 startPosition = new Vector3(position.x, position.y - optionHeight / 2, position.z);

            for (int i = 0; i < actions.Count; i++)
            {
                string action = actions[i];
                GameObject newButton = Instantiate(optionPrefab, contextMenu.transform).gameObject;

                TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = action;
                }

                RectTransform optionTransform = newButton.GetComponent<RectTransform>();
                optionTransform.anchoredPosition = new Vector2(0, -i * (optionHeight + spacing));
                Debug.Log("Created option: " + action);

                OptionHandler handler = newButton.GetComponent<OptionHandler>();
                if (handler == null)
                {
                    handler = newButton.AddComponent<OptionHandler>();
                }
                handler.Setup(action, this);
            }

            RectTransform contextMenuTransform = contextMenu.GetComponent<RectTransform>();
            Debug.Log($"Context menu RectTransform size: {contextMenuTransform.sizeDelta}");
            contextMenuTransform.sizeDelta = new Vector2(contextMenuTransform.sizeDelta.x, actions.Count * (optionHeight + spacing));
            contextMenu.SetActive(true);
            Debug.Log("Context menu set to active.");
            Debug.Log($"Context menu active state: {contextMenu.activeSelf}");
            Debug.Log($"Context menu position: {contextMenu.transform.position}");
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
        optionClicked = true;
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
        rightClickAction.Disable();
        leftClickAction.Disable();
    }
}