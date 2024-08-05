using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    public Item item;
    public int quantity = 1;
    public bool isInInventory = false; // Track if this item is in the inventory or not

    public List<string> GetActions()
    {
        List<string> actions = new List<string> { "Examine" };
        Debug.Log("Checking whether item is in inventory: " + isInInventory);
        if (isInInventory)
        {
            actions.AddRange(new List<string> { "Use", "Drop" });
        }
        else
        {
            actions.Add("Pick Up");
        }

        return actions;
    }

    public void PerformAction(string action)
    {
        Debug.Log("PerformAction called with action: " + action);
        if (action == "Pick Up")
        {
            PickUpItem();
        }
        else if (action == "Drop")
        {
            DropItem();
        }
        else if (action == "Examine")
        {
            ExamineItem();
        }
        else if (action == "Use")
        {
            UseItem();
        }
        // Handle other actions
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

    private void DropItem()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null && item != null)
        {
            Debug.Log("Dropping item from inventory: " + item.itemName);
            inventory.DropItem(item, quantity);
            // You can also instantiate the item in the world if needed
        }
        else
        {
            Debug.LogError("Inventory not found or item is null.");
        }
    }

    private void ExamineItem()
    {
        Debug.Log("Examining item: " + item.itemName);
        // Implement examination logic, like displaying item details
    }

    private void UseItem()
    {
        Debug.Log("Using item: " + item.itemName);
        // Implement item usage logic
    }
}