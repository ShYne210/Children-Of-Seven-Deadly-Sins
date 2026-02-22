using UnityEngine;
using UnityEngine.InputSystem;

public class PlacePoint : MonoBehaviour
{
    [Header("Thiết lập nhiệm vụ")]
    [SerializeField] private string requiredItemName; // tên vật phẩm cần để mở
    [SerializeField] private GameObject pressEUI;     // UI chữ E
    [SerializeField] public float openDuration = 5f;  // thời gian mở ban đầu
    [SerializeField] private float openRange = 2f;    // bán kính cho phép mở

    [Header("Thiết lập tăng độ khó")]
    [SerializeField] private float enemySpeedIncrease = 0.2f; // tăng tốc mỗi lần hoàn thành
    [SerializeField] private float waitTimeDecrease = 0.5f;   // giảm thời gian chờ mỗi lần hoàn thành
    [SerializeField] private float minOpenDuration = 1f;      // giới hạn tối thiểu

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
            if (pressEUI != null)
            {
                pressEUI.SetActive(true);
                pressEUI.transform.LookAt(Camera.main.transform);
                pressEUI.transform.Rotate(0, 180, 0);
            }

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryOpen(player, inventory); // ✅ gom logic vào TryOpen
            }
        }
        else
        {
            if (isCompleted && pressEUI != null)
                pressEUI.SetActive(false);
        }
    }

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
            player.StartCoroutine(CompleteOpen(inventory)); // ✅ gọi coroutine
            FindFirstObjectByType<MessageUI>().ShowMessage("Đang mở chỗ bằng vật phẩm " + requiredItemName);
            return true;
        }
        else
        {
            FindFirstObjectByType<MessageUI>().ShowMessage("Bạn không có vật phẩm cần thiết: " + requiredItemName);
            return false;
        }
    }

    private System.Collections.IEnumerator CompleteOpen(PlayerInventory inventory)
    {
        Debug.Log("Bắt đầu đếm thời gian mở chỗ...");
        float timer = 0f;

        // Chỉ đơn giản đếm thời gian, không ngắt giữa chừng
        while (timer < openDuration)
        {
            timer += Time.deltaTime;
            yield return null;

            if (isCompleted || player == null)
            {
                Debug.Log("Mở chỗ bị ngắt giữa chừng!");
                yield break;
            }
        }


        // Sau khi đủ thời gian thì hoàn thành luôn
        if (!isCompleted)
        {

            inventory.RemoveItem(requiredItemName);
            isCompleted = true;

            if (pressEUI != null) pressEUI.SetActive(false);
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Debug.Log("Mở chỗ thành công!");
            FindFirstObjectByType<MessageUI>().ShowMessage("Mở chỗ thành công!");
            FindFirstObjectByType<QuestManager>().AddProgress();

            // ✅ tăng tốc enemy
            EnemyVision enemyVision = FindFirstObjectByType<EnemyVision>();
            if (enemyVision != null)
            {
                enemyVision.SetSpeed(enemyVision.moveSpeed + enemySpeedIncrease);
            }

            // ✅ giảm thời gian chờ cho các điểm sau
            openDuration = Mathf.Max(minOpenDuration, openDuration - waitTimeDecrease);
        }
    }
}
