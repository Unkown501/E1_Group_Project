using UnityEngine;
using UnityEngine.UI;

public class PipeButton : MonoBehaviour
{
    [SerializeField] public bool up = false;
    [SerializeField] public bool down = false;
    [SerializeField] public bool left = false;
    [SerializeField] public bool right = false;
    [SerializeField] GameObject upTexture;
    [SerializeField] GameObject downTexture;
    [SerializeField] GameObject leftTexture;
    [SerializeField] GameObject rightTexture;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTexture();
    }

    void UpdateTexture()
    {
        upTexture.SetActive(up);
        downTexture.SetActive(down);
        leftTexture.SetActive(left);
        rightTexture.SetActive(right);
    }

    public void Rotate()
    {
        bool temp = left;
        left = down;
        down = right;
        right = up;
        up = temp;
    }
}
