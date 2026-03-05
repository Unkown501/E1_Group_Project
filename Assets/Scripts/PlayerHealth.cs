using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance; // Static reference to the singleton
    public static event System.Action OnPlayerDeath;

    public int maxHealth = 100;
    public int currentHealth;
    public int maxBattery = 100;
    public int currentBattery;

    void Awake()
    {
        // Singleton Logic: Ensure only one PlayerHealth exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being deleted
        }
        else
        {
            Destroy(gameObject); // Kill the duplicate in the new scene
            return;
        }

        // Initialize health only once
        currentHealth = maxHealth;
        currentBattery = maxBattery;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
            Die();
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
    public void addBattery(int battery)
    {
        if (currentBattery + battery > maxBattery)
        {
            currentBattery = maxBattery;
        }
        else
        {
            currentBattery += battery;
        }
    }


    private void Die()
    {
        Debug.Log("Player has died.");
        // Note: Be careful with Destroy(gameObject) here. 
        // If the object is destroyed, the Singleton Instance becomes null.
        // You might want to just disable the player or trigger a reload instead.
        Destroy(gameObject);
    }
}