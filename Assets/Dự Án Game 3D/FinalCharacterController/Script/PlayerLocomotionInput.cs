using UnityEngine;
using UnityEngine.InputSystem;

namespace DuAnGame3D.FinalCharacterController
{
    public class PlayerLocomotionInput 
        : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        private PlayerControls playerControls;

        private void Awake()
        {
            playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            playerControls.PlayerLocomotionMap.Enable();
            playerControls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            playerControls.PlayerLocomotionMap.RemoveCallbacks(this);
            playerControls.PlayerLocomotionMap.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        // 🔥 BẮT BUỘC PHẢI CÓ
        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }
    }
}
