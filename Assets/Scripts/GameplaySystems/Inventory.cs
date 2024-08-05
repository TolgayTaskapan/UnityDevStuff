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

    public void DropItem(Item item, int quantity)
    {
        if (item == null || quantity <= 0)
        {
            Debug.LogError("Invalid item or quantity specified for dropping.");
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 forwardDirection = player.transform.forward;
            Vector3 initialDropPosition = playerPosition + forwardDirection * 1.5f + Vector3.up * 0.5f; // Start above potential ground interference

            RaycastHit groundHit;
            if (Physics.Raycast(initialDropPosition, Vector3.down, out groundHit))
            {
                Vector3 dropPosition = groundHit.point; // This is the ground hit point

                GameObject itemPrefab = item.itemPrefab;
                if (itemPrefab != null)
                {
                    GameObject droppedItem = Instantiate(itemPrefab, dropPosition, Quaternion.identity);
                    Collider itemCollider = droppedItem.GetComponent<Collider>();
                    if (itemCollider != null)
                    {
                        // Correct placement by adding half the collider's full height
                        float colliderFullHeight = itemCollider.bounds.size.y;
                        droppedItem.transform.position += Vector3.up * colliderFullHeight * 0.5f;

                        ItemInteractable itemInteractable = droppedItem.GetComponent<ItemInteractable>();
                        if (itemInteractable != null)
                        {
                            itemInteractable.item = item;
                            itemInteractable.quantity = quantity;
                            itemInteractable.isInInventory = false;
                            Debug.Log($"Dropped {quantity} of {item.itemName} at position {droppedItem.transform.position}");
                        }
                        else
                        {
                            Debug.LogError("ItemInteractable component not found on the dropped item prefab.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Item prefab is null or does not have a collider component.");
                    }
                }
                else
                {
                    Debug.LogError("Item prefab is null. Cannot instantiate dropped item.");
                }
            }
            else
            {
                Debug.LogWarning("No ground detected below initial drop position.");
            }
        }
        else
        {
            Debug.LogError("Player object not found in the scene.");
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;
}