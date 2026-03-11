using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    private Canvas canvasObject;

    void Awake()
    {
        canvasObject = GetComponent<Canvas>();
        if (IsPaused)
            Pause();
        else
            Resume();
    }

    public void TogglePause()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        canvasObject.enabled = true;
        IsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        canvasObject.enabled = false;
        IsPaused = false;
    }
}
