using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [SerializeField] private TMP_Text prompt;
    protected bool playerInRange = false;
    protected float OriginalFontSize;

    protected virtual void Start()
    {
        if (prompt != null)
            OriginalFontSize = prompt.fontSize;

            prompt.text = "!!!";
            prompt.fontSize = 96;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (prompt != null)
                prompt.text = "INTERACT (E)";
                prompt.fontSize = OriginalFontSize;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (prompt != null)
                prompt.text = "!!!";
                prompt.fontSize = 128;
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    public bool CanInteract()
    {
        return playerInRange;
    }
}
