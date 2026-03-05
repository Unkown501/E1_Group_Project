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
    private float drainTimer = 0f;

    Vector2 lastMoveDir = Vector2.up;

    void Awake()
    {
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

        // Rotate to face movement direction
        if (flashlight != null)
        {
            float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
            flashlight.localRotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
        }
    }

    void FixedUpdate()
    {
        Vector2 delta = new Vector2(movementX, movementY) * speed * Time.fixedDeltaTime;
        transform.position = (Vector2)transform.position + delta;
    }
}