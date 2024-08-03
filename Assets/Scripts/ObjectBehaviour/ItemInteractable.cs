using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    public Item item;
    public int quantity = 1;

    public List<string> GetActions()
    {
        return new List<string> { "Pick Up", "Examine", "Use" };
    }

    public void PerformAction(string action)
    {
        Debug.Log("PerformAction called with action: " + action);
        if (action == "Pick Up")
        {
            PickUpItem();
        }
        // Handle other actions like Examine, Use, etc.
    }

    private void PickUpItem()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null && item != null)
        {
            Debug.Log("Adding item to inventory: " + item.itemName);
            bool added = inventory.AddItem(item, quantity);
            if (added)
            {
                Destroy(gameObject); // Remove the item from the scene
            }
            else
            {
                Debug.Log("Inventory is full.");
                // Show a UI message or other feedback to the player.
            }
        }
        else
        {
            Debug.LogError("Inventory not found or item is null.");
        }
    }
}
