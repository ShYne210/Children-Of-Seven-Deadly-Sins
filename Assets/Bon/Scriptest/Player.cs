using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("===== Trạng thái =====")]
    public bool isBusy = false;
    public bool hasItem = false;
    public bool enemyBusy = false;

    [Header("===== Busy Timer =====")]
    public float busyDuration = 5f;
    public float busyTimer = 0f;

    private PlacePoint currentPlacePoint;
    private ItemPickup currentItemPickup;
    private PlayerInventory inventory;

    void Start() { inventory = GetComponent<PlayerInventory>(); }

    void Update()
    {
        if (Keyboard.current == null) return;
        hasItem = inventory.ItemCount > 0;
        enemyBusy = hasItem || isBusy;

        if (isBusy && busyTimer > 0f)
        {
            if (Keyboard.current.eKey.isPressed)
            {
                busyTimer -= Time.deltaTime;
                if (busyTimer <= 0f) isBusy = false;
            }
            else isBusy = false;
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentItemPickup != null) currentItemPickup.Pickup(inventory);
            if (currentPlacePoint != null && currentPlacePoint.TryOpen(this, inventory))
                SetBusy(currentPlacePoint.openDuration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemPickup pickup)) currentItemPickup = pickup;
        if (other.TryGetComponent(out PlacePoint place)) currentPlacePoint = place;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ItemPickup pickup) && pickup == currentItemPickup) currentItemPickup = null;
        if (other.TryGetComponent(out PlacePoint place) && place == currentPlacePoint) currentPlacePoint = null;
    }

    public void SetBusy(float duration)
    {
        isBusy = true;
        busyDuration = duration;
        busyTimer = duration;
    }
}
