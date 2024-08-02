using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContextMenuManager : MonoBehaviour
{
    public GameObject contextMenu; // Reference to the context menu panel
    public TextMeshProUGUI optionPrefab; // Prefab for the text options
    private Camera cam;
    private GameObject currentTarget; // The object that was right-clicked

    void Start()
    {
        cam = Camera.main;
        contextMenu.SetActive(false); // Hide the context menu at start
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click detection
        {
            Vector3 screenPosition = Input.mousePosition;

            Ray ray = cam.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Set the current target and show the context menu
                currentTarget = hit.collider.gameObject;
                ShowContextMenu(screenPosition, currentTarget);
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2)) // Left or middle click
        {
            // Hide the context menu on any other click
            contextMenu.SetActive(false);
        }
    }

    void ShowContextMenu(Vector3 position, GameObject target)
{
    // Clear existing options
    foreach (Transform child in contextMenu.transform)
    {
        Destroy(child.gameObject);
    }

    // Check if the object implements IInteractable
    IInteractable interactable = target.GetComponent<IInteractable>();
    if (interactable != null)
    {
        List<string> actions = interactable.GetActions();

        // Calculate the starting position and offset for the options
        float optionHeight = optionPrefab.rectTransform.sizeDelta.y;
        float spacing = 5f; // Space between options
        Vector3 startPosition = new Vector3(position.x, position.y - optionHeight / 2, position.z);

        // Create a text option for each action
        for (int i = 0; i < actions.Count; i++)
        {
            string action = actions[i];
            TextMeshProUGUI option = Instantiate(optionPrefab, contextMenu.transform);
            option.text = action;
            option.color = Color.blue; // Customize the color as desired
            option.fontSize = 24; // Customize the font size as desired

            // Position each option with spacing
            RectTransform optionTransform = option.GetComponent<RectTransform>();
            optionTransform.anchoredPosition = new Vector2(0, -i * (optionHeight + spacing));

            // Set up the handler for clicks
            OptionHandler handler = option.gameObject.AddComponent<OptionHandler>();
            handler.Setup(action, this);
        }

        // Adjust context menu size and show it
        RectTransform contextMenuTransform = contextMenu.GetComponent<RectTransform>();
        contextMenuTransform.sizeDelta = new Vector2(contextMenuTransform.sizeDelta.x, actions.Count * (optionHeight + spacing));
        contextMenu.SetActive(true);
        contextMenu.transform.position = position;
    }
}

    public void OnActionSelected(string action)
    {
        Debug.Log("Action selected: " + action);

        // Perform action logic based on the selected action and currentTarget
        switch (action)
        {
            case "Talk":
                // Logic for talking to NPCs
                Debug.Log("Talk to NPC action");
                break;
            case "Examine":
                // Logic for examining objects
                Debug.Log("Examine action");
                contextMenu.SetActive(false);
                break;
            case "Pick Up":
                Debug.Log("Pick Up action");
                contextMenu.SetActive(false);
                // Logic for picking up items
                break;
            case "Use":
                // Logic for using items
                Debug.Log("Use item action");
                contextMenu.SetActive(false);
                break;
            // Add more cases for additional actions
        }

        contextMenu.SetActive(false);
    }
}