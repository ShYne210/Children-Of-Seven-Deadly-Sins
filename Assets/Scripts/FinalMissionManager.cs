using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FinalMissionManager : MonoBehaviour
{
    public static FinalMissionManager instance;

    [Header("Mission")]
    public int totalMachines = 4;

    // ✅ VÙNG CHUYỂN SCENE
    public EscapeZone exitDoor;

    // ✅ MODEL CỬA (nếu muốn ẩn cửa)
    public GameObject doorVisual;

    [Header("UI")]
    public TextMeshProUGUI orderText;

    [SerializeField]
    private List<int> correctOrder = new List<int>();

    private int currentIndex = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenerateOrder();

        // ✅ BAN ĐẦU KHÓA ESCAPE
        if (exitDoor != null)
            exitDoor.LockEscape();
    }

    // ======================
    // RANDOM ORDER
    // ======================
    void GenerateOrder()
    {
        correctOrder.Clear();
        currentIndex = 0;

        for (int i = 1; i <= totalMachines; i++)
            correctOrder.Add(i);

        Shuffle(correctOrder);

        UpdateOrderUI();

        Debug.Log("ORDER: " + string.Join(",", correctOrder));
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    // ======================
    // UI
    // ======================
    void UpdateOrderUI()
    {
        if (orderText == null) return;

        string text = "Repair Order:\n";

        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (i < currentIndex)
                text += "<color=green>[✔]</color> ";
            else
                text += "[ ] ";

            text += correctOrder[i] + "   ";
        }

        orderText.text = text;
    }

    // ======================
    // MACHINE CHECK
    // ======================
    public bool RepairMachine(int machineID)
    {
        if (currentIndex >= correctOrder.Count)
            return false;

        if (machineID == correctOrder[currentIndex])
        {
            Debug.Log("CORRECT MACHINE: " + machineID);

            currentIndex++;

            UpdateOrderUI();

            if (UINotification.instance)
                UINotification.instance.ShowSuccess();

            if (currentIndex >= totalMachines)
                CompleteMission();

            return true;
        }
        else
        {
            Debug.Log("WRONG ORDER → RESET");

            currentIndex = 0;

            UpdateOrderUI();

            if (UINotification.instance)
                UINotification.instance.ShowFail();

            return false;
        }
    }

    // ======================
    // COMPLETE MISSION
    // ======================
    void CompleteMission()
    {
        Debug.Log("FINAL COMPLETE");

        if (orderText)
            orderText.text =
            "<color=yellow>POWER RESTORED</color>";

        // ✅ ẨN CỬA (nếu có)
        if (doorVisual != null)
            doorVisual.SetActive(false);

        // ✅ MỞ ESCAPE ZONE
        if (exitDoor != null)
            exitDoor.UnlockEscape();
    }
}