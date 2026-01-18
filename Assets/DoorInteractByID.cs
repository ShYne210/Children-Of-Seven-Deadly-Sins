using UnityEngine;
using Seagull.Interior_04E.SceneProps;

public class DoorInteractByID : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] private RotatableObject rotatableObject;
    [SerializeField] private string doorID = "Door";
    [SerializeField] private float interactDistance = 3f;

    [Header("UI")]
    [SerializeField] private GameObject pressEText;

    private Rotatable doorRotatable;

    private void Awake()
    {
        // Lấy đúng Rotatable theo ID
        doorRotatable = rotatableObject
            .GetComponentInChildren<Rotatable>();
    }

    private void Update()
    {
        // UI chỉ để hiển thị
        if (pressEText != null)
            pressEText.SetActive(IsPlayerLookingAtDoor());
    }

    public void Interact()
    {
        if (!IsPlayerLookingAtDoor()) return;

        bool isOpen = doorRotatable.rotation > 0.5f;
        rotatableObject.rotate(doorID, isOpen ? 0f : 1f);
    }

    private bool IsPlayerLookingAtDoor()
    {
        Camera cam = Camera.main;
        if (cam == null) return false;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            return hit.collider.GetComponentInParent<RotatableObject>() == rotatableObject;
        }

        return false;
    }
}
