using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    float movementX;
    float movementY;

    [SerializeField] float speed = 2.0f;

    [Header("Flashlight Settings")]
    [SerializeField] Transform flashlight;
    [SerializeField] Light2D flashlightLight;
    [SerializeField] float angleOffset = -90f;

    // New Battery Variables
    [Tooltip("How many seconds it takes to drain 1 unit of battery")]
    [SerializeField] float batteryDrainRate = 1.0f;
    [SerializeField] float batteryAddRate = 1.0f;
    private float drainTimer = 0f;
    private float addTimer = 0f;

    // New Sprint Variables
    [Header("Sprint Settings")]
    [SerializeField] float sprintMultiplier = 1.75f;
    [Tooltip("How many seconds it takes to drain 1 unit of stamina while sprinting")]
    [SerializeField] float staminaDrainRate = 0.15f;
    [Tooltip("How many seconds it takes to regen 1 unit of stamina while not sprinting")]
    [SerializeField] float staminaAddRate = 0.10f;
    private float staminaDrainTimer = 0f;
    private float staminaAddTimer = 0f;

    Vector2 lastMoveDir = Vector2.up;

    public Animator anim;

    void Awake()
    {
        // Animator
        anim = GetComponent<Animator>();

        // Flashlight
        if (flashlight == null)
            flashlight = transform.Find("Flashlight");

        if (flashlightLight == null && flashlight != null)
            flashlightLight = flashlight.GetComponent<Light2D>();

        if (flashlightLight == null)
            Debug.LogError("No Light2D found on Player/Flashlight. Select Flashlight and make sure it has a Light2D component.");
    }

    void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        movementX = move.x;
        movementY = move.y;

        if (move.sqrMagnitude > 0.001f)
            lastMoveDir = move.normalized;
    }

    void Update()
    {
        // Toggle with F (New Input System friendly)
        if (flashlightLight != null && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            // If it's off, only allow turning it on if we have battery
            if (!flashlightLight.enabled)
            {
                if (PlayerHealth.Instance.currentBattery > 0)
                {
                    flashlightLight.enabled = true;
                }
                else
                {
                    Debug.Log("Cannot turn on flashlight: Out of battery!");
                }
            }
            else // If it's currently on, turn it off
            {
                flashlightLight.enabled = false;
            }
        }

        // Handle Battery Drain
        if (flashlightLight != null && flashlightLight.enabled)
        {
            // Check if we still have battery
            if (PlayerHealth.Instance.currentBattery > 0)
            {
                drainTimer += Time.deltaTime;

                // Once the timer hits our drain rate, lose 1 battery and reset timer
                if (drainTimer >= batteryDrainRate)
                {
                    PlayerHealth.Instance.LoseBattery();
                    drainTimer = 0f;
                }
            }
            else
            {
                // Battery hit 0 while the flashlight was on, force it off
                flashlightLight.enabled = false;
                Debug.Log("Flashlight died!");
            }
        }
        else
        {
            addTimer += Time.deltaTime;

            // Once the timer hits our drain rate, add 1 battery and reset timer
            if (addTimer >= batteryAddRate)
            {
                PlayerHealth.Instance.addBattery(1);
                addTimer = 0f;
            }
        }

        // Rotate to face movement direction
        if (flashlight != null)
        {
            float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
            flashlight.localRotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
        }

        // Update player animation state
        anim.SetFloat("speed", Mathf.Abs(movementX) + Mathf.Abs(movementY));

        if (movementX != 0 || movementY != 0)
        {
            anim.SetFloat("moveX", movementX);
            anim.SetFloat("moveY", movementY);
        }
    }

    void FixedUpdate()
    {
        // Sprint with Shift
        bool wantsSprint = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        bool isMoving = Mathf.Abs(movementX) + Mathf.Abs(movementY) > 0.01f;

        float moveSpeed = speed;

        if (wantsSprint && isMoving && PlayerHealth.Instance != null && PlayerHealth.Instance.HasStamina())
        {
            moveSpeed *= sprintMultiplier;

            staminaDrainTimer += Time.fixedDeltaTime;

            // Drain 1 stamina every staminaDrainRate seconds while sprinting
            if (staminaDrainTimer >= staminaDrainRate)
            {
                PlayerHealth.Instance.LoseStamina();
                staminaDrainTimer = 0f;
            }

            // Reset regen timer while sprinting
            staminaAddTimer = 0f;
        }
        else
        {
            // Regen stamina while not sprinting
            if (PlayerHealth.Instance != null)
            {
                staminaAddTimer += Time.fixedDeltaTime;

                // Regen 1 stamina every staminaAddRate seconds
                if (staminaAddTimer >= staminaAddRate)
                {
                    PlayerHealth.Instance.addStamina(1);
                    staminaAddTimer = 0f;
                }
            }

            // Reset drain timer while not sprinting
            staminaDrainTimer = 0f;
        }

        Vector2 delta = new Vector2(movementX, movementY) * moveSpeed * Time.fixedDeltaTime;
        transform.position = (Vector2)transform.position + delta;
    }
}