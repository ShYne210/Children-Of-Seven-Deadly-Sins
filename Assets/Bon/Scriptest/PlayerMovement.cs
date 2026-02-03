using UnityEngine;
using UnityEngine.InputSystem; // dùng Input System mới

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    private Player playerAction; // tham chiếu tới script Player để check isBusy

    [SerializeField] private Transform playerBody; // Thêm dòng này ở đầu class

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAction = GetComponent<Player>();
    }

    void Update()
    {
        // Nếu player đang busy thì không cho di chuyển
        if (playerAction != null && playerAction.isBusy)
        {
            velocity = Vector3.zero;
            return;
        }

        // Lấy input từ bàn phím
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
        }

        // Tạo vector di chuyển theo hướng playerBody
        Vector3 move = playerBody.right * input.x + playerBody.forward * input.y;
        move.y = 0f;
        move = move.normalized;

        // Di chuyển bằng CharacterController
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Thêm gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // giữ player dính mặt đất
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Xoay mặt theo hướng di chuyển
        // if (move != Vector3.zero)
        // {
        //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 10f);
        // }
    }
}
