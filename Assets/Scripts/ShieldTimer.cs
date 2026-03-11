using UnityEngine;
using TMPro;

public class ShieldTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text objectiveText;

    [SerializeField] private float startTime = 180f;

    private float currentTime;
    private bool timerRunning = true;
    private bool allTerminalsComplete = false;

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
            CheckAllObjectivesComplete();
            UpdateObjectiveDisplay();
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
        if (allTerminalsComplete)
        {
            objectiveText.text = "Restore power by searching the ship";
            return;
        }

        int terminal1 = MinigameState.CompletionStatus["Terminal1"] ? 1 : 0;
        int terminal2 = MinigameState.CompletionStatus["Terminal2"] ? 1 : 0;
        int terminal3 = MinigameState.CompletionStatus["Terminal3"] ? 1 : 0;

        objectiveText.text =
            "Main Console Active: " + terminal1 + "/1\n" +
            "Shield Console Active: " + terminal2 + "/1\n" +
            "Restore Power Flow: " + terminal3 + "/1";
    }

    void TimerFinished()
    {
        timerText.text = "Shield Fix In: 00:00";
        Debug.Log("Timer finished.");

        if (!allTerminalsComplete)
        {
            Debug.Log("Player failed to restore the shield in time.");

            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.KillPlayer();
            }
        }
    }

    void CheckAllObjectivesComplete()
    {
        if (MinigameState.CompletionStatus["Terminal1"] == true &&
            MinigameState.CompletionStatus["Terminal2"] == true &&
            MinigameState.CompletionStatus["Terminal3"] == true)
        {
            if (!allTerminalsComplete)
            {
                allTerminalsComplete = true;
                timerRunning = false;
                Debug.Log("All shield objectives completed!");
                UpdateObjectiveDisplay();
            }
        }
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        timerRunning = true;
        allTerminalsComplete = false;

        MinigameState.CompletionStatus["Terminal1"] = false;
        MinigameState.CompletionStatus["Terminal2"] = false;
        MinigameState.CompletionStatus["Terminal3"] = false;

        UpdateTimerDisplay();
        UpdateObjectiveDisplay();
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void StartTimer()
    {
        if (!allTerminalsComplete)
        {
            timerRunning = true;
        }
    }
}