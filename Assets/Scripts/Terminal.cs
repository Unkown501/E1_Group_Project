using UnityEngine;
using UnityEngine.SceneManagement;

public class Terminal : Interactable
{
    [SerializeField] string InitialState;
    [SerializeField] string GameName;

    public override void Interact()
    {
        MinigameState.InitialState = InitialState;
        SceneManager.LoadScene(GameName);
    }
}
