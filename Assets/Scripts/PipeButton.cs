using UnityEngine;
using UnityEngine.UI;

public class PipeButton : MonoBehaviour
{
    [SerializeField] public bool up = false;
    [SerializeField] public bool down = false;
    [SerializeField] public bool left = false;
    [SerializeField] public bool right = false;
    [SerializeField] Sprite ISprite;
    [SerializeField] Sprite LSprite;
    [SerializeField] Sprite TSprite;
    [SerializeField] Sprite XSprite;

    private Image buttonImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTexture();
    }

    void UpdateTexture()
    {
        buttonImage.enabled = true;

        // X
        if (up && right && down && left)
        {
            buttonImage.sprite = XSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        // T
        if (!up && right && down && left)
        {
            buttonImage.sprite = TSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        if (up && right && down && !left)
        {
            buttonImage.sprite = TSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            return;
        }
        if (up && right && !down && left)
        {
            buttonImage.sprite = TSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 180);
            return;
        }
        if (up && !right && down && left)
        {
            buttonImage.sprite = TSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 270);
            return;
        }

        // I
        if (up && !right && down && !left)
        {
            buttonImage.sprite = ISprite;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        if (!up && right && !down && left)
        {
            buttonImage.sprite = ISprite;
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            return;
        }

        // L
        if (up && right && !down && !left)
        {
            buttonImage.sprite = LSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        if (up && !right && !down && left)
        {
            buttonImage.sprite = LSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            return;
        }
        if (!up && !right && down && left)
        {
            buttonImage.sprite = LSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 180);
            return;
        }
        if (!up && right && down && !left)
        {
            buttonImage.sprite = LSprite;
            transform.localRotation = Quaternion.Euler(0, 0, 270);
            return;
        }

        buttonImage.enabled = false;
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
