using UnityEngine;

public class Generator : Interactable
{
    bool isActivated = false;
    [SerializeField] GameObject flame;

    void Update()
    {
        flame.SetActive(isActivated);
    }


    public override void Interact()
    {
        Debug.Log("Toggled Generator");
        isActivated = !isActivated;
    }

}
