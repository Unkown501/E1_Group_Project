using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class JumpscarePlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject jumpscareUI;
    [SerializeField] private MonoBehaviour playerMovementScript;

    private bool isPlaying = false;

    void Awake()
    {
        if (jumpscareUI != null)
        {
            jumpscareUI.SetActive(false);
        }

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.Stop();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.Stop();
        }
    }

    public void PlayJumpscare()
    {
        if (isPlaying) return;
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        isPlaying = true;

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        if (jumpscareUI != null)
        {
            jumpscareUI.SetActive(true);
        }

        Time.timeScale = 0f;

        if (videoPlayer != null)
        {
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }

            videoPlayer.Play();
        }

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }

        while (true)
        {
            bool videoDone = videoPlayer == null || !videoPlayer.isPlaying;
            bool audioDone = audioSource == null || !audioSource.isPlaying;

            if (videoDone && audioDone)
            {
                break;
            }

            yield return null;
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Time.timeScale = 1f;

        if (jumpscareUI != null)
        {
            jumpscareUI.SetActive(false);
        }

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        isPlaying = false;
    }
}