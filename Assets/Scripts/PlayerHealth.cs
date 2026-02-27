using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public static event System.Action OnPlayerDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Player health initialized to " + currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
           OnPlayerDeath?.Invoke();
           Die();
            
        }
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
    }
    private void Die()
    {
        // Handle player death here
        Destroy(gameObject);
        Debug.Log("Player has died.");
    }
}   