using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneLoader : MonoBehaviour
{
    [Header("Scene Loading Settings")]
    [SerializeField] public string sceneToLoad;
    [SerializeField] public Vector2 targetSpawnPosition;

    // Static variables survive even if this specific loader object is destroyed during the scene change
    private static Vector2 nextSpawnPosition;
    private static bool shouldMovePlayer = false;

    public void LoadScene()
    {
        // 1. Store the target coordinates in the static variable
        nextSpawnPosition = targetSpawnPosition;
        shouldMovePlayer = true;

        // 2. Subscribe to the event that fires right after a scene finishes loading
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 3. Load the new scene
        SceneManager.LoadScene(sceneToLoad);
    }

    // This method automatically triggers exactly when the new scene is ready
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure we actually want to move the player, and that the player exists
        if (shouldMovePlayer && PlayerHealth.Instance != null)
        {
            // Move the persistent player to the new coordinates
            PlayerHealth.Instance.transform.position = nextSpawnPosition;

            // Reset the flag so we don't accidentally move them again later
            shouldMovePlayer = false;
        }

        // Unsubscribe from the event to keep things clean and prevent errors
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
