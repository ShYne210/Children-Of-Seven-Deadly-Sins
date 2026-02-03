using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform playerBody; // Tham chiếu tới thân Player
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float verticalClamp = 90f; // Giới hạn góc nhìn lên/xuống

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        // Xoay ngang cho playerBody (xoay trục Y)
        playerBody.Rotate(Vector3.up * mouseX);

        // Xoay dọc cho camera (xoay trục X)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
