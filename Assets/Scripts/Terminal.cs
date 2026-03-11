using UnityEngine;
using UnityEngine.SceneManagement;

public class Terminal : Interactable
{
    [SerializeField] string InitialState;
    [SerializeField] string GameName;

    public override void Interact()
    {
        MinigameState.InitialState = InitialState;
        MinigameState.ReturnScene = SceneManager.GetActiveScene().name;
        MinigameState.ReturnPosition = (Vector2)transform.position;
        SceneManager.LoadScene(GameName);
    }
}
