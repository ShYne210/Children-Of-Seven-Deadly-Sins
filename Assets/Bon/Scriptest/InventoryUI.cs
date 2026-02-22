using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slot1Text;
    [SerializeField] private TextMeshProUGUI slot2Text;

    public void UpdateInventoryUI(List<string> items)
    {
        // Slot 1
        if (items.Count > 0)
            slot1Text.text = items[0];
        else
            slot1Text.text = "";

        // Slot 2
        if (items.Count > 1)
            slot2Text.text = items[1];
        else
            slot2Text.text = "";
    }
}
