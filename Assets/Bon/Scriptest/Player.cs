using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool isBusy = false;     // busy khi đang làm nhiệm vụ
    public bool hasItem = false;    // có vật phẩm trong inventory
    public bool enemyBusy = false;  // trạng thái để enemy nhận diện

    public float busyDuration = 5f;
    public float busyTimer = 0f;

    private PlacePoint currentPlacePoint;
    private ItemPickup currentItemPickup;
    private PlayerInventory inventory;

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Cập nhật trạng thái có item
        hasItem = inventory.ItemCount > 0;

        // Enemy nhận diện busy nếu player có item hoặc đang làm nhiệm vụ
        enemyBusy = hasItem || isBusy;

        // Nếu đang busy (ví dụ mở PlacePoint)
        if (isBusy && busyTimer > 0f)
        {
            if (Keyboard.current.eKey.isPressed)
            {
                busyTimer -= Time.deltaTime;
                if (busyTimer <= 0f)
                {
                    isBusy = false;
                    Debug.Log("Hoàn thành sau " + busyDuration + "s!");
                }
            }
            else
            {
                isBusy = false;
                Debug.Log("Đã ngắt giữa chừng!");
            }
            return;
        }

        // Nhặt item → chỉ đánh dấu có item
        if (Keyboard.current.eKey.wasPressedThisFrame && currentItemPickup != null)
        {
            currentItemPickup.Pickup(inventory);
            hasItem = true;
            enemyBusy = true;
            Debug.Log("Player đã nhặt item!");
        }

        // Dùng PlacePoint → busy thật sự
        if (Keyboard.current.eKey.wasPressedThisFrame && currentPlacePoint != null)
        {
            if (currentPlacePoint.TryOpen(this, inventory))
            {
                SetBusy(currentPlacePoint.openDuration);
                Debug.Log("Player bắt đầu mở chỗ bằng vật phẩm!");
            }
            else
            {
                Debug.Log("Player chưa có vật phẩm cần thiết để mở!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemPickup pickup))
            currentItemPickup = pickup;

        if (other.TryGetComponent(out PlacePoint place))
            currentPlacePoint = place;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ItemPickup pickup) && pickup == currentItemPickup)
            currentItemPickup = null;

        if (other.TryGetComponent(out PlacePoint place) && place == currentPlacePoint)
            currentPlacePoint = null;
    }

    public void SetBusy(float duration)
    {
        isBusy = true;
        busyDuration = duration;
        busyTimer = duration;
    }
}
