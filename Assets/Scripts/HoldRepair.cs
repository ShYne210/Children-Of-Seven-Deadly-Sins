using UnityEngine;
using UnityEngine.UI;

public class HoldRepair : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactText;
    public GameObject repairPanel;
    public Slider repairBar;
    public GameObject completeText;

    [Header("Repair")]
    public float repairTime = 3f;

    [Header("Manager")]
    public PowerManager manager;

    float timer = 0f;
    bool playerNear = false;
    bool repaired = false;

    void Update()
    {
        if (!playerNear || repaired)
            return;

        if (Input.GetKey(KeyCode.E))
        {
            repairPanel.SetActive(true);

            timer += Time.deltaTime;
            repairBar.value = timer / repairTime;

            if (timer >= repairTime)
                FinishRepair();
        }
        else
        {
            repairPanel.SetActive(false);
        }
    }

    void FinishRepair()
    {
        repaired = true;

        repairPanel.SetActive(false);
        interactText.SetActive(false);
        completeText.SetActive(true);

        if (manager != null)
            manager.MachineRepaired();


        GetComponent<Collider>().enabled = false;

        Invoke(nameof(HideComplete), 2f);
    }

    void HideComplete()
    {
        completeText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !repaired)
        {
            playerNear = true;

            interactText.SetActive(true);

            timer = 0;
            repairBar.value = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;

            interactText.SetActive(false);
            repairPanel.SetActive(false);

            timer = 0;
            repairBar.value = 0;
        }
    }

}