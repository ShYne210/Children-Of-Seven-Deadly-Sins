using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("===== UI Inventory =====")]
    [SerializeField] private TextMeshProUGUI slot1Text;
    [SerializeField] private TextMeshProUGUI slot2Text;

    public void UpdateInventoryUI(List<string> items)
    {
        slot1Text.text = items.Count > 0 ? items[0] : "";
        slot2Text.text = items.Count > 1 ? items[1] : "";
    }
}
