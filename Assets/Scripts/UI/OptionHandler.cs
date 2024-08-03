using UnityEngine;
using UnityEngine.UI; // Import for Button
using TMPro; // Import for TextMeshProUGUI

public class OptionHandler : MonoBehaviour
{
    private string action;
    private ContextMenuManager contextMenuManager;

    public void Setup(string action, ContextMenuManager contextMenuManager)
    {
        this.action = action;
        this.contextMenuManager = contextMenuManager;

        // Get the Button component attached to this GameObject
        Button button = GetComponent<Button>();

        // Check if Button component is present
        if (button != null)
        {
            // Remove any previous listeners to avoid multiple triggers
            button.onClick.RemoveAllListeners();
            // Add the OnButtonClicked method as a listener for the onClick event
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button component missing on OptionPrefab. Please attach a Button component.");
        }
    }

    private void OnButtonClicked()
    {
        Debug.Log("Button clicked: " + action);
        contextMenuManager.OnActionSelected(action);
    }
}