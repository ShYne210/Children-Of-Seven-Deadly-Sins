using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // ================= GAME 1 =================
    [Header("Game 1 Progress")]
    public bool game1Completed = false;
    public GameObject winNotificationUI;

    // ================= FINAL PUZZLE =================
    [Header("Final Puzzle")]
    public bool finalGameStarted = false;

    public int repairedCount = 0;
    public int totalMachines = 4;

    public GameObject exitDoor;

    void Awake()
    {
        instance = this;
    }

    // ================= GAME 1 WIN =================
    public void CompleteGame1()
    {
        game1Completed = true;

        Debug.Log("Game 1 Completed");

        if (winNotificationUI != null)
            winNotificationUI.SetActive(true);

        Invoke(nameof(HideWinUI), 3f);
    }

    void HideWinUI()
    {
        if (winNotificationUI != null)
            winNotificationUI.SetActive(false);
    }

    // ================= FINAL GAME =================
    public void StartFinalGame()
    {
        finalGameStarted = true;
        Debug.Log("Final Game Started");
    }

    // ================= MACHINE REPAIR =================
    public void MachineRepaired()
    {
        repairedCount++;

        Debug.Log("Machines Fixed: " + repairedCount);

        if (repairedCount >= totalMachines)
        {
            OpenExitDoor();
        }
    }

    void OpenExitDoor()
    {
        Debug.Log("EXIT OPENED");

        if (exitDoor != null)
            exitDoor.SetActive(false);
    }
}