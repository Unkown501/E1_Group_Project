using UnityEngine;

public class DoorwayScript : MonoBehaviour
{
    [SerializeField] NewSceneLoader sceneLoader;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip doorSound;

    private bool hasTriggered = false;

    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (audioSource != null && doorSound != null)
            {
                audioSource.PlayOneShot(doorSound);
            }

            sceneLoader.LoadScene();
        }
    }

    void Update()
    {
    }
}