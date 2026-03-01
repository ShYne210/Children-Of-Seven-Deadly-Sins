using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    [Header("===== Thiết lập Item =====")]
    [SerializeField] private string itemName = "DefaultItem";
    [SerializeField] private GameObject pressEUI;
    [SerializeField] private float pickupRange = 3f;

    private Transform player;
    private PlayerInventory inventory;
    private bool isPicked = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventory = player.GetComponent<PlayerInventory>();
        if (pressEUI != null) pressEUI.SetActive(false);
    }

    void Update()
    {
        if (isPicked || player == null) return;
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= pickupRange)
        {
            if (pressEUI != null)
            {
                pressEUI.SetActive(true);
                pressEUI.transform.LookAt(Camera.main.transform);
                pressEUI.transform.Rotate(0, 180, 0);
            }

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (inventory.AddItem(itemName))
                {
                    isPicked = true;
                    if (pressEUI != null) pressEUI.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
        else if (pressEUI != null) pressEUI.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }

    public bool Pickup(PlayerInventory inventory)
    {
        inventory.AddItem(itemName);
        Destroy(gameObject);
        return true;
    }
}
