using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void OnEnable()
    {
        // Subscribe when the object is enabled
        PlayerHealth.OnPlayerDeath += HandleGameOver;
    }

    void OnDisable()
    {
        // ALWAYS unsubscribe to prevent errors when switching scenes
        PlayerHealth.OnPlayerDeath -= HandleGameOver;
    }

    void HandleGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}