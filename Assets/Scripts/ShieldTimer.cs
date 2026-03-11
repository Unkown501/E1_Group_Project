using UnityEngine;
using TMPro;

public class ShieldTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private float startTime = 180f; // 3 minutes

    private float currentTime;
    private bool timerRunning = true;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
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
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerFinished()
    {
        timerText.text = "00:00";
        Debug.Log("3 minutes completed");

        // Put your shield regeneration or fix logic here
        // Example:
        // playerShield.RepairShield();
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        timerRunning = true;
        UpdateTimerDisplay();
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