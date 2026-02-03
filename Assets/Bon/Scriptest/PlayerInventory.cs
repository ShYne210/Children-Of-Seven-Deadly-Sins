using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private List<string> items = new List<string>();

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("Player đã nhặt: " + itemName);
    }

    public void RemoveItem(string itemName)
    {
        if (items.Contains(itemName))
        {
            items.Remove(itemName);
            Debug.Log("Player đã mất item: " + itemName);
        }
        else
        {
            Debug.Log("Player không có item để xóa: " + itemName);
        }
    }

    public bool HasItemAtIndex(int index)
    {
        return index >= 0 && index < items.Count;
    }

    public string GetItemAtIndex(int index)
    {
        if (HasItemAtIndex(index))
            return items[index];
        return null;
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    private void SomeMethod()
    {
        string requiredItemName = "SomeItem";
        if (HasItem(requiredItemName))
        {
            Debug.Log("Player has the required item: " + requiredItemName);
        }
    }
}
