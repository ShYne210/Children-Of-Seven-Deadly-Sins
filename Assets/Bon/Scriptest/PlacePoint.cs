using UnityEngine;
using UnityEngine.InputSystem;

public class PlacePoint : MonoBehaviour
{
    [Header("===== Nhiệm vụ =====")]
    [SerializeField] private string requiredItemName;
    [SerializeField] private GameObject pressEUI;
    [SerializeField] public float openDuration = 5f;
    [SerializeField] private float openRange = 2f;

    [Header("===== Độ khó =====")]
    [SerializeField] private float enemySpeedIncrease = 0.2f;
    [SerializeField] private float waitTimeDecrease = 0.5f;
    [SerializeField] private float minOpenDuration = 1f;

    public bool isCompleted = false;
    private bool playerInRange = false;
    private PlayerInventory inventory;
    private Player player;

    void Start() { if (pressEUI != null) pressEUI.SetActive(false); }

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
            playerInRange = false; player = null; inventory = null;
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
                TryOpen(player, inventory);
        }
        else if (isCompleted && pressEUI != null) pressEUI.SetActive(false);
    }

    public bool TryOpen(Player player, PlayerInventory inventory)
    {
        if (isCompleted) return false;
        if (Vector3.Distance(player.transform.position, transform.position) > openRange)
        {
            FindFirstObjectByType<MessageUI>().ShowMessage("Bạn đứng quá xa!");
            return false;
        }
        if (inventory.HasItem(requiredItemName))
        {
            player.SetBusy(openDuration);
            player.StartCoroutine(CompleteOpen(inventory));
            FindFirstObjectByType<MessageUI>().ShowMessage("Đang mở bằng " + requiredItemName);
            return true;
        }
        FindFirstObjectByType<MessageUI>().ShowMessage("Thiếu vật phẩm: " + requiredItemName);
        return false;
    }

    private System.Collections.IEnumerator CompleteOpen(PlayerInventory inventory)
    {
        float timer = 0f;
        while (timer < openDuration)
        {
            timer += Time.deltaTime;
            yield return null;
            if (isCompleted || player == null) yield break;
        }

        if (!isCompleted)
        {
            inventory.RemoveItem(requiredItemName);
            isCompleted = true;
            if (pressEUI != null) pressEUI.SetActive(false);
            GetComponent<Collider>().enabled = false;

            FindFirstObjectByType<MessageUI>().ShowMessage("Mở thành công!");
            FindFirstObjectByType<QuestManager>().AddProgress();

            EnemyVision enemyVision = FindFirstObjectByType<EnemyVision>();
            if (enemyVision != null) enemyVision.SetSpeed(enemyVision.moveSpeed + enemySpeedIncrease);

            openDuration = Mathf.Max(minOpenDuration, openDuration - waitTimeDecrease);
        }
    }
}
