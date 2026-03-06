using UnityEngine;
using UnityEngine.UI;

public class PlayerBarsUI : MonoBehaviour
{
    [Header("Fill Images")]
    [SerializeField] private Image healthFill;
    [SerializeField] private Image batteryFill;
    [SerializeField] private Image staminaFill;

    void Update()
    {
        var p = PlayerHealth.Instance;
        if (p == null) return;

        healthFill.fillAmount = Safe01(p.currentHealth, p.maxHealth);
        batteryFill.fillAmount = Safe01(p.currentBattery, p.maxBattery);
        staminaFill.fillAmount = Safe01(p.currentStamina, p.maxStamina);
    }

    float Safe01(int cur, int max)
    {
        if (max <= 0) return 0f;
        return Mathf.Clamp01((float)cur / max);
    }
}