using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    

    void Start()
    {
        PlayerHealth.OnPlayerDeath += HandleGameOver;
    }

    void HandleGameOver()
    {
        // Load game over screen, stop time, show UI, etc.
        SceneManager.LoadScene("GameOver");
    }
}
