using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.Instance;
        if (inventory != null)
        {
            // Subscribe to the inventory changed event
            inventory.InventoryChanged += UpdateUI;
        }

        UpdateUI();
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            // Unsubscribe from the event when the object is destroyed
            inventory.InventoryChanged -= UpdateUI;
        }
    }

    public void UpdateUI()
    {
        // Clear existing slots
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new slots based on inventory data
        foreach (InventorySlot slot in inventory.inventorySlots)
        {
            if (slot.item != null)
            {
                GameObject slotObj = Instantiate(slotPrefab, inventoryPanel.transform);

                // Ensure the ItemIcon and Quantity components exist and are correctly assigned
                Image icon = slotObj.transform.Find("ItemIcon")?.GetComponent<Image>();
                TextMeshProUGUI quantityText = slotObj.transform.Find("Quantity")?.GetComponent<TextMeshProUGUI>();

                if (icon != null && slot.item.icon != null)
                {
                    icon.sprite = slot.item.icon;
                    icon.enabled = true;
                }
                else
                {
                    if (icon != null)
                        icon.enabled = false;
                }

                if (quantityText != null)
                {
                    quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
                }
                else
                {
                    Debug.LogError("Quantity TextMeshProUGUI component not found in the slot prefab.");
                }
            }
        }
    }
}