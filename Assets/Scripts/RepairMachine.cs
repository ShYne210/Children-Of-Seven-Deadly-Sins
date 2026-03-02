using UnityEngine;
using UnityEngine.UI;

public class RepairMachine : MonoBehaviour
{
    public int machineID;

    public GameObject repairUI;
    public Image progressBar;

    float holdTime = 0;
    public float repairDuration = 3f;

    bool playerNear = false;

    void Update()
    {
        if (!playerNear) return;

        if (Input.GetKey(KeyCode.E))
        {
            holdTime += Time.deltaTime;

            progressBar.fillAmount =
                holdTime / repairDuration;

            if (holdTime >= repairDuration)
                CompleteRepair();
        }

        if (Input.GetKeyUp(KeyCode.E))
            ResetHold();
    }

    // ==========================
    void CompleteRepair()
    {
        repairUI.SetActive(false);

        bool correct =
            FinalMissionManager.instance
            .RepairMachine(machineID);

        // ✅ luôn reset để sửa lại được
        ResetHold();

        // hiện UI lại nếu player còn đứng gần
        if(playerNear)
            repairUI.SetActive(true);
    }

    // ==========================
    void ResetHold()
    {
        holdTime = 0;

        if(progressBar != null)
            progressBar.fillAmount = 0;
    }

    // ==========================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            repairUI.SetActive(true);
            playerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            repairUI.SetActive(false);
            playerNear = false;
            ResetHold();
        }
    }
}