using UnityEngine;
using UnityEngine.SceneManagement;

public class Terminal : Interactable
{
    [SerializeField] string TerminalID;
    [SerializeField] string InitialState;
    [SerializeField] string GameName;

    void Update()
    {
        // Turn off if already complete
        if (MinigameState.CompletionStatus[TerminalID])
        {
            gameObject.SetActive(false);
        }
    }

    public override void Interact()
    {
        // Ignore if already complete
        if (MinigameState.CompletionStatus[TerminalID])
        {
            return;
        }
        MinigameState.TerminalID = TerminalID;
        MinigameState.InitialState = InitialState;
        MinigameState.ReturnScene = SceneManager.GetActiveScene().name;
        MinigameState.ReturnPosition = (Vector2)transform.position;
        SceneManager.LoadScene(GameName);
    }
}
