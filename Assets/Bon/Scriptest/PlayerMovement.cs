using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("===== Di chuyển =====")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float gravity = -9.81f;

    [Header("===== Stamina =====")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrain = 20f;
    [SerializeField] private float staminaRegen = 10f;
    [SerializeField] private Slider staminaBar;

    [Header("===== Âm thanh bước chân =====")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private float stepInterval = 0.5f;
    [SerializeField] private float walkVolume = 0.6f;
    [SerializeField] private float runVolume = 1.0f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.95f, 1.05f);

    [Header("===== Âm thanh thở =====")]
    [SerializeField] private AudioSource breathSource;
    [SerializeField] private AudioClip breathClip;

    private float currentStamina;
    private CharacterController controller;
    private Vector3 velocity;
    private Player playerAction;
    [SerializeField] private Transform playerBody;

    private float stepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAction = GetComponent<Player>();
        currentStamina = maxStamina;

        if (staminaBar != null)
            staminaBar.maxValue = maxStamina;

        stepTimer = 0f;
    }

    void Update()
    {
        if (playerAction != null && playerAction.isBusy)
        {
            velocity = Vector3.zero;
            return;
        }

        // Input WASD
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
        }

        Vector3 move = playerBody.right * input.x + playerBody.forward * input.y;
        move.y = 0f;
        move = move.normalized;

        // Kiểm tra chạy
        bool isTryingRun = Keyboard.current.leftShiftKey.isPressed;
        bool canRun = currentStamina > 0;
        bool isRunning = isTryingRun && canRun;

        float speed = isRunning ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // Stamina logic
        if (isRunning)
        {
            currentStamina = Mathf.Max(0, currentStamina - staminaDrain * Time.deltaTime);
            StopBreath();
        }
        else
        {
            if (!isTryingRun && staminaRegen > 0 && currentStamina < maxStamina)
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegen * Time.deltaTime);
                PlayBreath();
            }
            else
            {
                StopBreath();
            }
        }

        if (staminaBar != null)
            staminaBar.value = currentStamina;

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Âm thanh bước chân
        HandleFootsteps(move, isRunning);
    }

    private void HandleFootsteps(Vector3 move, bool isRunning)
    {
        if (move.magnitude > 0.1f && controller.isGrounded)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                footstepSource.clip = isRunning ? runClip : walkClip;
                footstepSource.volume = isRunning ? runVolume : walkVolume;
                footstepSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
                footstepSource.Play();
                stepTimer = stepInterval;
            }
        }
        else
        {
            if (footstepSource.isPlaying)
                footstepSource.Stop();
            stepTimer = 0f;
        }
    }

    private void PlayBreath()
    {
        if (breathSource != null && breathClip != null && !breathSource.isPlaying)
        {
            breathSource.clip = breathClip;
            breathSource.loop = true;
            breathSource.Play();
        }
    }

    private void StopBreath()
    {
        if (breathSource != null && breathSource.isPlaying)
        {
            breathSource.Stop();
        }
    }
}
