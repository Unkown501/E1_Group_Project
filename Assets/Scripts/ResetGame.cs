using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public void Reset()
    {
        // Reset MinigameState
        MinigameState.CompletionStatus.Clear();

        // Reset DontDestroyOnLoad

        GameObject temp = new GameObject();
        DontDestroyOnLoad(temp);

        Scene ddolScene = temp.scene;

        foreach (GameObject obj in ddolScene.GetRootGameObjects())
        {
            Object.Destroy(obj);
        }
    }

    
}
