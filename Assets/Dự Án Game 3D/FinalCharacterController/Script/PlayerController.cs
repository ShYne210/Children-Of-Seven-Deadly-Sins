using UnityEngine;

namespace DuAnGame3D.FinalCharacterController
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerLocomotionInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraPivot;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Mouse Look")]
        [SerializeField] private float mouseSensitivity = 1.5f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;

        private PlayerLocomotionInput input;
        private Vector3 verticalVelocity;
        private float cameraPitch;

        private void Awake()
        {
            input = GetComponent<PlayerLocomotionInput>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMovement();
            HandleMouseLook();
            ApplyGravity();
        }

        private void HandleMovement()
        {
            Vector2 move = input.MovementInput;

            Vector3 moveDir =
                transform.right * move.x +
                transform.forward * move.y;

            characterController.Move(moveDir * moveSpeed * Time.deltaTime);
        }

        private void HandleMouseLook()
        {
            // 👉 CHỈ DÙNG INPUT SYSTEM
            Vector2 look = input.LookInput * mouseSensitivity;

            cameraPitch -= look.y;
            cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
            cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

            transform.Rotate(Vector3.up * look.x);
        }

        private void ApplyGravity()
        {
            if (characterController.isGrounded && verticalVelocity.y < 0)
                verticalVelocity.y = -2f;

            verticalVelocity.y += gravity * Time.deltaTime;
            characterController.Move(verticalVelocity * Time.deltaTime);
        }
    }
}
