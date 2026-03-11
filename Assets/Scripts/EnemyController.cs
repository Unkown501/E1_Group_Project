using Pathfinding;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class EnemyController : MonoBehaviour
{
    private Transform goal;
    public enum MovementType
    {
        DirectChase,
        LineOfSight,
        RushDown
    }
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private MovementType _MovementType;
    [SerializeField] private float hitStunDuration = 1.0f; // How long the enemy stops for
    public float viewDistance = 10f;
    public bool lightSensitive = false;
    public bool lightStunable = false;
    private Light2D flashlight;
    private bool inLight = false;
    private bool isStunned = false;

    AIPath ai;
    Rigidbody2D rb;
    float wanderTimer;
    Vector3 wanderTarget;

    void Awake()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) {
            goal = playerObj.transform;
            Transform lightTransform = goal.Find("Flashlight"); 
            if (lightTransform != null) {
                flashlight = lightTransform.GetComponent<Light2D>();
            }
        }
    }
    // Triggered when the enemy touches the player's collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            // Use the Singleton instance to apply damage
            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(damageAmount);
                StartCoroutine(StopMovement());
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ai = GetComponent<AIPath>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned) return;
        switch (_MovementType)
        {
            case MovementType.DirectChase:
                DoDirectChase();
                break;
            case MovementType.LineOfSight:
                DoLineOfSight();
                break;
            case MovementType.RushDown:
                DoRushDown();
                break;
        }
    }
    bool CheckLight()
    {
        if (!lightSensitive || flashlight.isActiveAndEnabled)
        {
            return true;
        }
        return false;
    }
    bool Stunned()
    {
        if (inLight && lightStunable && flashlight.isActiveAndEnabled)
        {
            return true;
        }
        return false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered Flashlight Trigger");
            inLight = true;
        if (other.CompareTag("Player"))
        {
            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(damageAmount);
                StartCoroutine(StopMovement());
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exited Flashlight Trigger");
            inLight = false;
    }
    void DoDirectChase()
    {
        float distance = Vector2.Distance(rb.position, goal.position);
        Vector2 direction = ((Vector2)goal.position - rb.position).normalized;
        int layerMask = LayerMask.GetMask("Walls");
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, distance, layerMask);

        if (hit.collider == null && distance <= viewDistance && CheckLight() && !Stunned())
        {
            ai.destination = goal.position;
        }
        if (Stunned())
        {
            ai.destination = rb.position;
        }
    }

    void DoLineOfSight()
    {
        Vector2 direction = ((Vector2)goal.position - rb.position).normalized;
        float distance = Vector2.Distance(rb.position, goal.position);
        int layerMask = LayerMask.GetMask("Walls");

        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, distance, layerMask);

        if (hit.collider == null && distance <= viewDistance && !CheckLight() && !Stunned())
        {
            ai.destination = goal.position;
        }
        else
        {
            Wander();
        }
        if (Stunned())
        {
            ai.destination = rb.position;
        }
    }

    void DoRushDown()
    {
        Vector2 direction = ((Vector2)goal.position - rb.position).normalized;
        float distance = Vector2.Distance(rb.position, goal.position);
        int layerMask = LayerMask.GetMask("Walls");

        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, distance, layerMask);

        if (hit.collider == null && distance <= viewDistance && CheckLight() && !Stunned())
        {
            StartCoroutine(Rush(direction));
        }
        else
        {
            Wander();
        }
        if (Stunned())
            {
                ai.destination = rb.position;
            }
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            wanderTimer = Random.Range(2f, 5f);
            wanderTarget = rb.position + Random.insideUnitCircle * 5f;
        }
        ai.destination = wanderTarget;
    }

    IEnumerator Rush(Vector2 direction)  // cant stun after finished charging
    {
        if (Stunned())
        {
            yield break;
        }
        yield return new WaitForSeconds(3f);

        ai.enabled = false;
        float speed = 15f;
        int layerMask = LayerMask.GetMask("Walls", "Player");
        float elapsed = 0f;

        while (elapsed < 2f)
        {
            RaycastHit2D hit = Physics2D.CircleCast(rb.position, 0.5f, direction, rb.linearVelocity.magnitude * Time.fixedDeltaTime, layerMask);
            if (hit.collider != null)
                break;

            rb.linearVelocity = direction * speed;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        rb.linearVelocity = Vector2.zero;
        ai.enabled = true;
    }
    private IEnumerator StopMovement()
    {
        isStunned = true;

        // Kill the enemy's current velocity so they don't slide into the player
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        yield return new WaitForSeconds(hitStunDuration);

        isStunned = false;
    }
}
