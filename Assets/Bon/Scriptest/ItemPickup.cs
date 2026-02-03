using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string itemName = "DefaultItem";
    [SerializeField] private GameObject pressEUI; // UI chữ E
    [SerializeField] private float pickupRange = 3f; // phạm vi hiển thị E

    private Transform player;
    private PlayerInventory inventory;
    private bool isPicked = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventory = player.GetComponent<PlayerInventory>();

        if (pressEUI != null)
            pressEUI.SetActive(false); // ẩn lúc đầu
    }

    void Update()
    {
        if (isPicked || player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // Nếu player trong phạm vi
        if (dist <= pickupRange)
        {
            if (pressEUI != null) pressEUI.SetActive(true);

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                inventory.AddItem(itemName);
                Debug.Log("Player đã nhặt: " + itemName);
                isPicked = true;

                if (pressEUI != null) pressEUI.SetActive(false);
                Destroy(gameObject); // xóa item sau khi nhặt
            }
        }
        else
        {
            if (pressEUI != null) pressEUI.SetActive(false);
        }
    }

    // Vẽ phạm vi trong Scene view để dễ chỉnh
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
    public void Pickup(PlayerInventory inventory) 
    { 
        inventory.AddItem(itemName); 
        Debug.Log("Đã nhặt item: " + itemName); 
        FindFirstObjectByType<QuestManager>().AddProgress();
        Destroy(gameObject); // xoá vật phẩm khỏi scene sau khi nhặt 
    }

    
}
