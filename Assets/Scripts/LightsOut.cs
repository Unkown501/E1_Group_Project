using UnityEngine;
using UnityEngine.UI;

public class LightsOut : MonoBehaviour
{

    [SerializeField] GameObject lightPrefab;
    [SerializeField] GameObject lightsContainer;

    [SerializeField] int nRows = 4;
    [SerializeField] int nCols = 4;
    [SerializeField] int[,] initialState;

    private RectTransform containerRect;




    private GameObject[,] lights;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lights = new GameObject[nRows, nCols];

        containerRect = GetComponent<RectTransform>();
        float height = containerRect.rect.height;
        float width = containerRect.rect.width;

        float gapX = (width - nCols*100)/(4-1);
        float gapY = (height - nRows*100)/(4-1);

        float offsetX = -0.5f*width + 50;
        float offsetY = -0.5f*height + 50;

        for (int row = 0; row < lights.GetLength(0); row++)
        {
            for (int col = 0; col < lights.GetLength(1); col++)
            {
                GameObject newLight = Instantiate(lightPrefab, lightsContainer.transform);
                newLight.transform.localPosition = new Vector3(offsetX + (100+gapX)*col, offsetY + (100+gapY)*row, 0);
                newLight.transform.localRotation = Quaternion.identity;

                lights[row, col] = newLight;

                int newRow = row;
                int newCol = col;

                newLight.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(newRow, newCol));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnButtonClicked(int row, int col)
    {
        Debug.Log($"Clicked button at [{row}, {col}]");

        lights[row, col].GetComponent<LightButton>().Toggle();

        if (row-1 >= 0)
        {
            lights[row-1, col].GetComponent<LightButton>().Toggle();
        }
        if (col-1 >= 0)
        {
            lights[row, col-1].GetComponent<LightButton>().Toggle();
        }
        if (row+1 < nRows)
        {
            lights[row+1, col].GetComponent<LightButton>().Toggle();
        }
        if (col+1 < nCols)
        {
            lights[row, col+1].GetComponent<LightButton>().Toggle();
        }

        if (isAllOn())
        {
            Debug.Log("COMPLETED!");
        }
    }


    public bool isAllOn()
    {
        for (int row = 0; row < lights.GetLength(0); row++)
        {
            for (int col = 0; col < lights.GetLength(1); col++)
            {
                if (!lights[row, col].GetComponent<LightButton>().lit)
                {
                    return false;
                }
            }
        }

        return true;
    }

}
