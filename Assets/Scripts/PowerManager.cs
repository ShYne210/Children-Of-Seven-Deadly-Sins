using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public int totalMachines = 3;

    private int repairedMachines = 0;

    [Header("After Complete")]
    public GameObject doorBlock;
    public GameObject puzzleActivator;

    public void MachineRepaired()
    {
        repairedMachines++;

        Debug.Log("Machine Fixed: " + repairedMachines);

        if (repairedMachines >= totalMachines)
        {
            Debug.Log("POWER RESTORED");

            if (doorBlock != null)
                doorBlock.SetActive(false);

            if (puzzleActivator != null)
                puzzleActivator.SetActive(true);

            // ⭐⭐⭐ QUAN TRỌNG NHẤT
            GameManager.instance.CompleteGame1();
        }
    }
}