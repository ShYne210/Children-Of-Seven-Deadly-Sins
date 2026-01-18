using UnityEngine;
using UnityEngine.InputSystem;

namespace DuAnGame3D.FinalCharacterController
{
    public class PlayerLocomotionInput 
        : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        public static event System.Action OnInteractPressed;

        private PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();
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

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        // ⚠️ HÀM NÀY PHẢI KHỚP 100%
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                DoorInteractByID door =
                    hit.collider.GetComponentInParent<DoorInteractByID>();

                if (door != null)
                    door.Interact();
            }
        }
    }
}
