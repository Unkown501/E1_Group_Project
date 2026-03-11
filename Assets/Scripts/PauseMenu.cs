using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    void Awake()
    {
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
        gameObject.SetActive(true);
        IsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        IsPaused = false;
    }
}
