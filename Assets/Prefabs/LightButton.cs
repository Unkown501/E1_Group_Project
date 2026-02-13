using UnityEngine;
using UnityEngine.UI;

public class LightButton : MonoBehaviour
{

    private Image btnImg;
    [SerializeField] Color litColor;
    [SerializeField] Color dimColor;

    public bool lit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btnImg = GetComponent<Image>();
        btnImg.color = lit ? litColor : dimColor;
    }

    // Update is called once per frame
    void Update()
    {
        btnImg.color = lit ? litColor : dimColor;
    }

    public void Toggle()
    {
        lit = !lit;
    }
}
