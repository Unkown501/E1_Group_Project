using UnityEngine;
using TMPro;

public class ShieldTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text objectiveText;

    [SerializeField] private float startTime = 180f;

    private float currentTime;
    private bool timerRunning = true;

    private int mainConsoleActive = 0;
    private int shieldConsoleActive = 0;
    private int powerFlowRestored = 0;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
        UpdateObjectiveDisplay();
    }

    void Update()
    {
        if (!timerRunning)
        {
            return;
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                currentTime = 0;
            }

            UpdateTimerDisplay();
        }
        else
        {
            timerRunning = false;
            TimerFinished();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = "Shield Fix In: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateObjectiveDisplay()
    {
        objectiveText.text =
            "Main Console Active: " + mainConsoleActive + "/1\n" +
            "Shield Console Active: " + shieldConsoleActive + "/1\n" +
            "Restore Power Flow: " + powerFlowRestored + "/1";
    }

    void TimerFinished()
    {
        timerText.text = "Shield Fix In: 00:00";
        Debug.Log("Timer finished.");

        // Put game result logic here
    }

    public void ActivateMainConsole()
    {
        mainConsoleActive = 1;
        UpdateObjectiveDisplay();
        CheckAllObjectivesComplete();
    }

    public void ActivateShieldConsole()
    {
        shieldConsoleActive = 1;
        UpdateObjectiveDisplay();
        CheckAllObjectivesComplete();
    }

    public void RestorePowerFlow()
    {
        powerFlowRestored = 1;
        UpdateObjectiveDisplay();
        CheckAllObjectivesComplete();
    }

    void CheckAllObjectivesComplete()
    {
        if (mainConsoleActive == 1 &&
            shieldConsoleActive == 1 &&
            powerFlowRestored == 1)
        {
            Debug.Log("All shield objectives completed!");
        }
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        timerRunning = true;

        mainConsoleActive = 0;
        shieldConsoleActive = 0;
        powerFlowRestored = 0;

        UpdateTimerDisplay();
        UpdateObjectiveDisplay();
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void StartTimer()
    {
        timerRunning = true;
    }
}