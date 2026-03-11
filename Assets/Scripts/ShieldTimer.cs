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
    private bool powerPillarsComplete = false;
    private bool gameCompleted = false;

    void Start()
    {
        ResetTimer();
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
        CheckPowerPillarsComplete();
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

        if (firstStageComplete && !powerPillarsComplete)
        {
            int power1 = MinigameState.CompletionStatus["Power1"] ? 1 : 0;
            int power2 = MinigameState.CompletionStatus["Power2"] ? 1 : 0;

            objectiveText.text =
                "Explore the ship\n" +
                "Turn on the power pillars without the fire\n" +
                "Power Pillar 1: " + power1 + "/1\n" +
                "Power Pillar 2: " + power2 + "/1";
            return;
        }

        if (powerPillarsComplete)
        {
            int powerTerminal1 = MinigameState.CompletionStatus["PowerTerminal1"] ? 1 : 0;
            int powerTerminal2 = MinigameState.CompletionStatus["PowerTerminal2"] ? 1 : 0;
            int powerTerminal3 = MinigameState.CompletionStatus["PowerTerminal3"] ? 1 : 0;

            objectiveText.text =
                "Power restored. Activate terminals\n" +
                "Power Terminal 1: " + powerTerminal1 + "/1\n" +
                "Power Terminal 2: " + powerTerminal2 + "/1\n" +
                "Power Terminal 3: " + powerTerminal3 + "/1";
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

    void CheckPowerPillarsComplete()
    {
        if (firstStageComplete && !powerPillarsComplete &&
            MinigameState.CompletionStatus["Power1"] == true &&
            MinigameState.CompletionStatus["Power2"] == true)
        {
            powerPillarsComplete = true;
            Debug.Log("Power pillars complete. Now activate power terminals.");
            UpdateObjectiveDisplay();
        }
    }

    void CheckSecondStageComplete()
    {
        if (powerPillarsComplete && !gameCompleted &&
            MinigameState.CompletionStatus["PowerTerminal1"] == true &&
            MinigameState.CompletionStatus["PowerTerminal2"] == true &&
            MinigameState.CompletionStatus["PowerTerminal3"] == true)
        {
            gameCompleted = true;
            Debug.Log("All power terminals completed. Loading Survived scene.");
            SceneManager.LoadScene("Survived");
        }
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        timerRunning = true;
        firstStageComplete = false;
        powerPillarsComplete = false;
        gameCompleted = false;

        MinigameState.CompletionStatus["Terminal1"] = false;
        MinigameState.CompletionStatus["Terminal2"] = false;
        MinigameState.CompletionStatus["Terminal3"] = false;

        MinigameState.CompletionStatus["Power1"] = false;
        MinigameState.CompletionStatus["Power2"] = false;

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
