using UnityEngine;

public class DoorByKey : MonoBehaviour
{
    public string requiredKeyID;
    public GameObject doorObject;

    private bool playerNear;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerInventori.instance.HasKey(requiredKeyID))
            {
                doorObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}