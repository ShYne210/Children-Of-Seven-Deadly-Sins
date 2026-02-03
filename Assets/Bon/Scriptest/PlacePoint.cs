using UnityEngine;
using UnityEngine.InputSystem;

public class PlacePoint : MonoBehaviour
{
    [SerializeField] private string requiredItemName; // tên vật phẩm cần để mở
    [SerializeField] private GameObject pressEUI;     // UI chữ E
    [SerializeField] public float openDuration = 5f;
    [SerializeField] private float openRange = 2f; // Bán kính cho phép mở
    public bool isCompleted = false;

    private bool playerInRange = false;
    private PlayerInventory inventory;
    private Player player;

    void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCompleted)
        {
            player = other.GetComponent<Player>();
            inventory = other.GetComponent<PlayerInventory>();
            playerInRange = true;
            if (pressEUI != null) pressEUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            inventory = null;
            if (pressEUI != null) pressEUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && !isCompleted && inventory != null && player != null)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (inventory.HasItem(requiredItemName))
                {
                    player.SetBusy(5f);

                    Debug.Log("Player bắt đầu mở chỗ bằng vật phẩm: " + requiredItemName);

                    if (pressEUI != null) pressEUI.SetActive(false);
                }
                else
                {
                    Debug.Log("Player chưa có vật phẩm " + requiredItemName + " để mở!");
                }
            }
        }
    }
    // public bool TryOpen(PlayerInventory inventory)
    // {
    //     if (isCompleted) return false;
    //     if (inventory.HasItem(requiredItemName))
    //     {
    //         Debug.Log("Đã mở chỗ bằng item: " + requiredItemName);
    //         isCompleted = true;
    //         return true;
    //     }
    //     else
    //     {
    //         Debug.Log("Không có item " + requiredItemName + " để mở!");
    //         return false;
    //     }
    // }
    // Gọi khi player nhấn E
    public bool TryOpen(Player player, PlayerInventory inventory)
    {
        if (isCompleted) return false;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > openRange)
        {
            FindFirstObjectByType<MessageUI>().ShowMessage("Bạn đứng quá xa để mở!");
            return false;
        }

        if (inventory.HasItem(requiredItemName))
        {
            player.SetBusy(openDuration);
            player.StartCoroutine(CompleteOpen(inventory));
            FindFirstObjectByType<MessageUI>().ShowMessage("Đang mở chỗ bằng vật phẩm " + requiredItemName);
            return true;
        }
        else
        {
            FindFirstObjectByType<MessageUI>().ShowMessage("Bạn không có vật phẩm cần thiết!");
            return false;
        }
    }


    private System.Collections.IEnumerator CompleteOpen(PlayerInventory inventory)
    {
        float timer = 0f;
        while (timer < openDuration)
        {
            // Nếu player bị ngắt giữa chừng (không còn busy hoặc đã completed), dừng lại và không xóa vật phẩm
            if (isCompleted || player == null || !player.isBusy)
            {
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo chỉ hoàn thành khi player vẫn còn busy
        if (!isCompleted && player != null)
            if (!isCompleted && player != null)
            {
                inventory.RemoveItem(requiredItemName);
                isCompleted = true;
                FindFirstObjectByType<MessageUI>().ShowMessage("Mở chỗ thành công!");
                FindFirstObjectByType<QuestManager>().AddProgress();
            }
    }
}
