using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    float movementX;
    float movementY;

    [SerializeField] float speed = 2.0f;

    [Header("Flashlight (Light2D on child named Flashlight)")]
    [SerializeField] Transform flashlight;
    [SerializeField] Light2D flashlightLight;   // <-- Light2D, not Light
    [SerializeField] float angleOffset = -90f;  // you needed -90 for your rotation issue

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
            flashlightLight.enabled = !flashlightLight.enabled;

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