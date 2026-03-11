using UnityEngine;
using System.Collections; // Required for Coroutines
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance; // Static reference to the singleton
    public static event System.Action OnPlayerDeath;

    public int maxHealth = 100;
    public int currentHealth;
    public int maxBattery = 100;
    public int currentBattery;
    public int maxShieldingLevel = 100;
    public int currentShieldingLevel;

    // New Sprint/Stamina Variables
    public int maxStamina = 100;
    public int currentStamina;

    [Header("I-Frame Settings")]
    [SerializeField] private float iFrameDuration = 1.5f;
    [SerializeField] private float flashInterval = 0.1f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Singleton Logic: Ensure only one PlayerHealth exists
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Prevent this object from being deleted
        }
        else
        {
            Destroy(gameObject); // Kill the duplicate in the new scene
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        // Initialize health only once
        currentHealth = maxHealth;
        currentBattery = maxBattery;

        // Initialize stamina / shield
        currentStamina = maxStamina;
        currentShieldingLevel = maxShieldingLevel;
    }

    public void TakeDamage(int damage)
    {

        if (isInvincible) return;
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
            Die();
        }
        else
        {
            // Start invincibility period
            StartCoroutine(BecomeInvincible());
        }
    }

    public void LoseBattery()
    {
        if (currentBattery > 0)
        {
            currentBattery--;
            Debug.Log("Battery Point Deleted. Current Battery: " + currentBattery);
        }
    }

    public void loseShield()
    {
        if (currentShieldingLevel > 0)
        {
            currentShieldingLevel--;
            Debug.Log("Shielding Level Depleted. Current Shielding Level: " + currentShieldingLevel);
        }
        else
        {
            OnPlayerDeath?.Invoke();
            Die();
        }
    }

    public void addShield(int shield)
    {
        if (currentShieldingLevel + shield > maxShieldingLevel)
            currentShieldingLevel = maxShieldingLevel;
        else
            currentShieldingLevel += shield;
    }

    public void addBattery(int battery)
    {
        if (currentBattery + battery > maxBattery)
        {
            currentBattery = maxBattery;
            Debug.Log("Battery Level Increased. Current Level: " + currentBattery);
        }
        else
        {
            currentBattery += battery;
            Debug.Log("Battery Level Increased. Current Level: " + currentBattery);
        }
    }

    // Sprint/Stamina Helpers
    public bool HasStamina()
    {
        return currentStamina > 0;
    }

    public void LoseStamina()
    {
        if (currentStamina > 0)
        {
            currentStamina--;
            Debug.Log("Stamina Depleted. Current Stamina: " + currentStamina);
        }
    }

    public void addStamina(int amount)
    {
        if (currentStamina + amount > maxStamina)
            currentStamina = maxStamina;
        else
            currentStamina += amount;
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Note: Be careful with Destroy(gameObject) here. 
        // If the object is destroyed, the Singleton Instance becomes null.
        // You might want to just disable the player or trigger a reload instead.
        Destroy(gameObject);
    }
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        // Visual feedback: Flash the player sprite
        float elapsed = 0f;
        while (elapsed < iFrameDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true; // Ensure sprite is visible at the end
        isInvincible = false;
    }
    public void KillPlayer()
    {
        OnPlayerDeath?.Invoke();
        Die();
    }
}