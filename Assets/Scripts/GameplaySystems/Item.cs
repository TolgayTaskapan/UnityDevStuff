using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public GameObject itemPrefab;
    public string itemName;
    public Sprite icon;
    public bool isStackable;
    public int maxStackSize;
}