using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OptionHandler : MonoBehaviour, IPointerClickHandler
{
    private string action;
    private ContextMenuManager contextMenuManager;

    public void Setup(string action, ContextMenuManager contextMenuManager)
    {
        this.action = action;
        this.contextMenuManager = contextMenuManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        contextMenuManager.OnActionSelected(action);
    }
}