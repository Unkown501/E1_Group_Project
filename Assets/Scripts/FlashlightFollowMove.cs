using UnityEngine;

public class FlashlightFollowMove : MonoBehaviour
{
    [Header("Flashlight")]
    public Transform flashlight;          // optional: assign in inspector
    public string flashlightTag = "Flashlight";

    [Header("Controls")]
    public KeyCode toggleKey = KeyCode.F;

    [Header("Rotation")]
    public bool useLastMoveWhenIdle = true;
    public float angleOffsetDegrees = 0f; // set to 90 if your beam points "up" by default

    private bool isOn = true;
    private Vector2 lastMoveDir = Vector2.right;

    void Awake()
    {
        if (flashlight == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag(flashlightTag);
            if (found != null) flashlight = found.transform;
        }
    }

    void Update()
    {
        // Toggle on/off
        if (flashlight != null && Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            flashlight.gameObject.SetActive(isOn);
        }

        // Movement direction from input (WASD/Arrows)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = new Vector2(x, y);

        if (moveDir.sqrMagnitude > 0.001f)
            lastMoveDir = moveDir.normalized;
        else if (!useLastMoveWhenIdle)
            return;

        // Rotate flashlight to face movement direction
        if (flashlight != null)
        {
            float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
            flashlight.localRotation = Quaternion.Euler(0f, 0f, angle + angleOffsetDegrees);
        }
    }
}