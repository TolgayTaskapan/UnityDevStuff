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
        if (isInInventory)
        {
            actions.Add("Use");
            if (quantity >= 1) actions.Add("Drop");
            if (quantity >= 5) actions.Add("Drop 5");
            if (quantity >= 10) actions.Add("Drop 10");
            if (quantity > 1) actions.Add("Drop All");
        }
        else
        {
            actions.Add("Pick Up");
        }
        return actions;
    }
    public void PerformAction(string action)
    {
        if (action == "Pick Up")
        {
            PickUpItem();
        }
        else if (action == "Drop")
        {
            DropItem(1);
        }
        else if (action == "Drop 5")
        {
            DropItem(5);
        }
        else if (action == "Drop 10")
        {
            DropItem(10);
        }
        else if (action == "Drop All")
        {
            DropItem(quantity);
        }
        else if (action == "Examine")
        {
            ExamineItem();
        }
        else if (action == "Use")
        {
            UseItem();
        }
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

    private void DropItem(int dropQuantity)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null && item != null)
        {
            inventory.DropItem(item, dropQuantity);
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