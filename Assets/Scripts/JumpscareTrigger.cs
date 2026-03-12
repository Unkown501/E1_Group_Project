using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    [SerializeField] private JumpscarePlayer jumpscarePlayer;
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && triggerOnce) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            jumpscarePlayer.PlayJumpscare();
            if (triggerOnce)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
