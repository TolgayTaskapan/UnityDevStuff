using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int inventorySize = 28;

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged InventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializeInventory();
    }

    private void InitializeInventory()
    {
        inventorySlots.Clear(); // Clear existing slots
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
        Debug.Log($"Inventory initialized with {inventorySlots.Count} empty slots.");
    }

    public bool AddItem(Item item, int quantity)
    {
        Debug.Log($"Adding {quantity} of {item.itemName}");

        // Check if the item is stackable and if it exists in the inventory
        if (item.isStackable)
        {
            InventorySlot slot = inventorySlots.Find(s => s.item == item);
            if (slot != null)
            {
                slot.quantity += quantity;
                Debug.Log($"Updated quantity for {item.itemName}: {slot.quantity}");
                InventoryChanged?.Invoke(); // Trigger the event
                return true;
            }
        }

        // If not stackable or not found, find an empty slot
        InventorySlot emptySlot = inventorySlots.Find(s => s.item == null);
        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = quantity;
            Debug.Log($"Set quantity for new item {item.itemName}: {quantity}");
            InventoryChanged?.Invoke(); // Trigger the event
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
            InventoryChanged?.Invoke(); // Trigger the event
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;
}