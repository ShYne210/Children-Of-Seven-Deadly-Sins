using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private List<string> items = new List<string>();
    private const int MAX_SLOTS = 1; // ✅ chỉ cho phép 1 slot

    public bool AddItem(string itemName)
    {
        if (items.Count >= MAX_SLOTS)
        {
            Debug.Log("Inventory đã đầy, không thể nhặt thêm!");
            FindFirstObjectByType<MessageUI>().ShowMessage("Túi đã đầy, chỉ chứa tối đa 1 vật phẩm!");
            return false;
        }

        items.Add(itemName);
        Debug.Log("Player đã nhặt: " + itemName);

        FindFirstObjectByType<InventoryUI>().UpdateInventoryUI(items);
        return true;
    }

    public void RemoveItem(string itemName)
    {
        if (items.Contains(itemName))
        {
            items.Remove(itemName);
            Debug.Log("Player đã mất item: " + itemName);

            FindFirstObjectByType<InventoryUI>().UpdateInventoryUI(items);
        }
    }

    public int ItemCount => items.Count;
    public bool HasItemAtIndex(int index) => index >= 0 && index < items.Count;
    public string GetItemAtIndex(int index) => HasItemAtIndex(index) ? items[index] : null;
    public bool HasItem(string itemName) => items.Contains(itemName);
}
