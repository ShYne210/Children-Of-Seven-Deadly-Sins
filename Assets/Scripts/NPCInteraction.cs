using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject guideCanvas;
    private bool playerInRange = false;

    void Start()
    {
        guideCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            guideCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            guideCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        Debug.Log("Đã tương tác với NPC");
    }
}
