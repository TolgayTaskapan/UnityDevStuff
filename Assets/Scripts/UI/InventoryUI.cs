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
    public Image icon;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        Image prefabIcon = slotPrefab.transform.Find("ItemIcon").GetComponent<Image>();
        if (prefabIcon != null)
        {
            icon = prefabIcon;
        }

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
        foreach (InventorySlot slot in inventory.inventorySlots) {
            GameObject slotObj = Instantiate(slotPrefab, inventoryPanel.transform);

            // Find and assign the Image component for the item icon
            Image icon = slotObj.transform.Find("ItemIcon").GetComponent<Image>();

            // Find and assign the TextMeshProUGUI component for the quantity
            TextMeshProUGUI quantityText = slotObj.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

            if (slot.item != null)
            {
                icon.sprite = slot.item.icon;
                icon.enabled = true;
                quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
            }
            else {
                icon.enabled = false;
                quantityText.text = "";
            }
        }    
    }

    public void SetItem(Sprite iconSprite) {
        if (iconSprite != null)
        {
            icon.sprite = iconSprite;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }
}