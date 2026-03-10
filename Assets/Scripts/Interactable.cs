using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject prompt;

    protected bool playerInRange = false;

    protected virtual void Start()
    {
        if (prompt != null)
            prompt.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (prompt != null)
                prompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (prompt != null)
                prompt.SetActive(false);
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
