using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool isBusy = false;
    public float busyDuration = 5f;
    public float busyTimer = 0f;

    private PlacePoint currentPlacePoint;   // chỗ mở hiện tại
    private ItemPickup currentItemPickup;   // item hiện tại
    private PlayerInventory inventory;

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Nếu đang busy thì chỉ cho phép đếm ngược và ngắt nếu nhả E
        if (isBusy)
        {
            // Nếu vẫn giữ phím E thì tiếp tục đếm
            if (Keyboard.current.eKey.isPressed)
            {
                busyTimer -= Time.deltaTime;
                if (busyTimer <= 0f)
                {
                    isBusy = false;
                    Debug.Log("Hoàn thành sau " + busyDuration + "s!");
                }
            }
            else // Nếu nhả phím E thì ngắt tiến trình
            {
                isBusy = false;
                Debug.Log("Đã ngắt giữa chừng!");
            }
            return;
        }

        if (Keyboard.current == null) return;

        // Nhặt item (không busy)
        if (Keyboard.current.eKey.wasPressedThisFrame && currentItemPickup != null)
        {
            currentItemPickup.Pickup(inventory);
        }

        // Dùng PlacePoint (busy)
        if (Keyboard.current.eKey.wasPressedThisFrame && currentPlacePoint != null)
        {
            if (currentPlacePoint.TryOpen(this, inventory))
            {
                SetBusy(currentPlacePoint.openDuration); // busy theo thời gian của PlacePoint
                Debug.Log("Player bắt đầu mở chỗ bằng vật phẩm!");
            }
            else
            {
                Debug.Log("Player chưa có vật phẩm cần thiết để mở!");
            }
        }

        // Debug item theo slot
        if (Keyboard.current.digit1Key.wasPressedThisFrame) DebugItem(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) DebugItem(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) DebugItem(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) DebugItem(3);
    }

    void DebugItem(int index)
    {
        if (inventory.HasItemAtIndex(index))
        {
            string itemName = inventory.GetItemAtIndex(index);
            Debug.Log("Player đã gọi item: " + itemName + " bằng phím " + (index + 1));
        }
        else
        {
            Debug.Log("Player chưa có item ở slot " + (index + 1));
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
