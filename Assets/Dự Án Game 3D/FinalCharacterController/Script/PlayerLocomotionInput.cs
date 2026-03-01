using UnityEngine;
using UnityEngine.InputSystem;

namespace DuAnGame3D.FinalCharacterController
{
    public class PlayerLocomotionInput : MonoBehaviour,
        PlayerControls.IPlayerLocomotionMapActions
    {
        // ===== MOVEMENT =====
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        [Header("Interact")]
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private Transform cameraTransform;

        private PlayerControls controls;

        // Lưu book đang mở (để đóng bằng E / ESC)
        private BookInteract currentBook;

        private void Awake()
        {
            controls = new PlayerControls();

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            controls.PlayerLocomotionMap.Enable();
            controls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            controls.PlayerLocomotionMap.RemoveCallbacks(this);
            controls.PlayerLocomotionMap.Disable();
        }

        // ===== INPUT CALLBACKS =====

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            // ===== NẾU ĐANG ĐỌC SÁCH → ĐÓNG SÁCH =====
            if (currentBook != null && currentBook.IsOpen)
            {
                currentBook.CloseBook();
                currentBook = null;
                return;
            }

            if (cameraTransform == null) return;

            Ray ray = new Ray(cameraTransform.position,
                              cameraTransform.forward);

            if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance))
                return;

            // ===== ƯU TIÊN BOOK =====
            BookInteract book = hit.collider.GetComponentInParent<BookInteract>();
            if (book != null)
            {
                currentBook = book;
                book.Interact();
                return;
            }

            // ===== SAU ĐÓ LÀ CỬA =====
            DoorInteractByID door =
                hit.collider.GetComponentInParent<DoorInteractByID>();

            if (door != null)
            {
                door.Interact();
            }
        }
    }
}
