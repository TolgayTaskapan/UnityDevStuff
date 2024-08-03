using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Clear existing slots
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new slots
        foreach (InventorySlot slot in inventory.inventorySlots)
        {
            GameObject slotObj = Instantiate(slotPrefab, inventoryPanel.transform);
            Image icon = slotObj.transform.Find("Icon").GetComponent<Image>();
            Text quantityText = slotObj.transform.Find("Quantity").GetComponent<Text>();

            if (slot.item != null)
            {
                icon.sprite = slot.item.icon;
                icon.enabled = true;
                quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
            }
            else
            {
                icon.enabled = false;
                quantityText.text = "";
            }
        }
    }
}