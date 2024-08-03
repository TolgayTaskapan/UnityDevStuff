using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int inventorySize = 28;

    private void Start()
    {
        // Initialize the inventory with empty slots
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item, int quantity)
    {
        // Check if item is stackable and if it exists in the inventory
        if (item.isStackable)
        {
            InventorySlot slot = inventorySlots.Find(s => s.item == item);
            if (slot != null)
            {
                slot.quantity += quantity;
                return true;
            }
        }

        // If not stackable or not found, find an empty slot
        InventorySlot emptySlot = inventorySlots.Find(s => s.item == null);
        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = quantity;
            return true;
        }

        // Inventory full
        return false;
    }

    public void RemoveItem(Item item, int quantity)
    {
        InventorySlot slot = inventorySlots.Find(s => s.item == item);
        if (slot != null)
        {
            slot.quantity -= quantity;
            if (slot.quantity <= 0)
            {
                slot.item = null;
                slot.quantity = 0;
            }
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;
}