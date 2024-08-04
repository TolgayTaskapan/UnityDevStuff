using UnityEngine;
using UnityEngine.UI;

public class OptionHandler : MonoBehaviour
{
    private string action;
    private ContextMenuManager contextMenuManager;

    public void Setup(string action, ContextMenuManager contextMenuManager)
    {
        this.action = action;
        this.contextMenuManager = contextMenuManager;
        Debug.Log($"[{gameObject.name}] Setup: Setting up button for action '{action}'.");
    }

    public void OnButtonClicked()
    {
        Debug.Log($"[{gameObject.name}] OnButtonClicked: Button for action '{action}' clicked.");
        if (contextMenuManager != null)
        {
            Debug.Log($"[{gameObject.name}] OnButtonClicked: Calling OnActionSelected on ContextMenuManager with action '{action}'.");
            contextMenuManager.OnActionSelected(action);
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] OnButtonClicked: ContextMenuManager is null. Cannot perform action '{action}'.");
        }
    }
}