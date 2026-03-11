using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShieldTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private float startTime = 180f;

    private float currentTime;
    private bool timerRunning = true;

    private bool firstStageComplete = false;
    private bool gameCompleted = false;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
        UpdateObjectiveDisplay();
    }

    void Update()
    {
        if (!gameCompleted && !firstStageComplete)
        {
            if (timerRunning)
            {
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
        }

        CheckFirstStageComplete();
        CheckSecondStageComplete();
        UpdateObjectiveDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = "Shield Fix In: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateObjectiveDisplay()
    {
        if (gameCompleted)
        {
            objectiveText.text = "Power restored.";
            return;
        }

        if (firstStageComplete)
        {
            int power1 = MinigameState.CompletionStatus["PowerTerminal1"] ? 1 : 0;
            int power2 = MinigameState.CompletionStatus["PowerTerminal2"] ? 1 : 0;
            int power3 = MinigameState.CompletionStatus["PowerTerminal3"] ? 1 : 0;

            objectiveText.text =
                "Explore the ship\n" +
                "Power Terminal 1: " + power1 + "/1\n" +
                "Power Terminal 2: " + power2 + "/1\n" +
                "Power Terminal 3: " + power3 + "/1";
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

        if (!firstStageComplete)
        {
            Debug.Log("Player failed to finish the shield tasks in time.");

            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.KillPlayer();
            }
        }
    }

    void CheckFirstStageComplete()
    {
        if (!firstStageComplete &&
            MinigameState.CompletionStatus["Terminal1"] == true &&
            MinigameState.CompletionStatus["Terminal2"] == true &&
            MinigameState.CompletionStatus["Terminal3"] == true)
        {
            firstStageComplete = true;
            timerRunning = false;
            Debug.Log("First stage complete. Explore the ship.");
            UpdateObjectiveDisplay();
        }
    }

    void CheckSecondStageComplete()
    {
        if (firstStageComplete && !gameCompleted &&
            MinigameState.CompletionStatus["PowerTerminal1"] == true &&
            MinigameState.CompletionStatus["PowerTerminal2"] == true &&
            MinigameState.CompletionStatus["PowerTerminal3"] == true)
        {
            gameCompleted = true;
            Debug.Log("All power terminals completed. Loading Survive scene.");
            SceneManager.LoadScene("Survived");
        }
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        timerRunning = true;
        firstStageComplete = false;
        gameCompleted = false;

        MinigameState.CompletionStatus["Terminal1"] = false;
        MinigameState.CompletionStatus["Terminal2"] = false;
        MinigameState.CompletionStatus["Terminal3"] = false;

        MinigameState.CompletionStatus["PowerTerminal1"] = false;
        MinigameState.CompletionStatus["PowerTerminal2"] = false;
        MinigameState.CompletionStatus["PowerTerminal3"] = false;

        UpdateTimerDisplay();
        UpdateObjectiveDisplay();
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void StartTimer()
    {
        if (!firstStageComplete && !gameCompleted)
        {
            timerRunning = true;
        }
    }
}