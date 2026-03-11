using UnityEngine;

public class Generator : Interactable
{
    bool isActivated = false;
    [SerializeField] GameObject flame;
    [SerializeField] string ID;


    void Update()
    {
        flame.SetActive(isActivated);
        MinigameState.CompletionStatus[ID] = isActivated;
    }


    public override void Interact()
    {
        Debug.Log("Toggled Generator");
        isActivated = !isActivated;
        MinigameState.CompletionStatus[ID] = isActivated;
    }

}
