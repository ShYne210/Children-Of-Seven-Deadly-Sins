using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.8f;
    public float jumpForce = 2f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 150f;
    public Transform playerCamera;

    CharacterController controller;

    float yVelocity;
    float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        if (controller.isGrounded && yVelocity < 0)
            yVelocity = -2f;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            yVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);

        yVelocity += gravity * Time.deltaTime;

        move.y = yVelocity;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void Look()
    {
        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity * Time.deltaTime;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.localRotation =
            Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX);
    }
}