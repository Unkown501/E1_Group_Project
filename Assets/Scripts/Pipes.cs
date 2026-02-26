using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Pipes : MonoBehaviour
{
    [SerializeField] GameObject pipePrefab;
    [SerializeField] GameObject pipesContainer;

    [SerializeField] int nRows = 4;
    [SerializeField] int nCols = 4;
    // [SerializeField] int[,] initialState;

    private RectTransform containerRect;

    private GameObject[,] pipes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pipes = new GameObject[nRows, nCols];
        
        containerRect = GetComponent<RectTransform>();
        float height = containerRect.rect.height;
        float width = containerRect.rect.width;

        float gapX = (width - nCols*100)/(nCols-1);
        float gapY = (height - nRows*100)/(nRows-1);

        float offsetX = -0.5f*width + 50;
        float offsetY = -0.5f*height + 50;

        for (int row = 0; row < pipes.GetLength(0); row++)
        {
            for (int col = 0; col < pipes.GetLength(1); col++)
            {
                GameObject newPipe = Instantiate(pipePrefab, pipesContainer.transform);
                newPipe.transform.localPosition = new Vector3(offsetX + (100+gapX)*col, offsetY + (100+gapY)*row, 0);
                newPipe.transform.localRotation = Quaternion.identity;

                pipes[row, col] = newPipe;

                int newRow = row;
                int newCol = col;

                newPipe.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(newRow, newCol));
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

        pipes[row, col].GetComponent<PipeButton>().Rotate();

        if (IsConnected(2, 2))
        {
            Debug.Log("COMPLETED!");
        }
    }

    public bool IsConnected(int startRow, int targetRow)
    {
        (int row, int col) targetPos = (targetRow, nCols-1);

        if (!pipes[startRow, 0].GetComponent<PipeButton>().left || !pipes[targetPos.row, targetPos.col].GetComponent<PipeButton>().right)
        {
            return false;
        }

        HashSet<(int row, int col)> visited = new HashSet<(int, int)>();
        Stack<(int row, int col)> toVisit = new Stack<(int, int)>();
        toVisit.Push((startRow, 0));
        
        while (toVisit.Count != 0)
        {
            (int row, int col) currPos = toVisit.Pop();
            visited.Add(currPos);

            // Check if found target
            if (currPos == targetPos)
            {
                return true;
            }

            // Check up
            if (currPos.row+1 < nRows && !visited.Contains((currPos.row+1, currPos.col)) && pipes[currPos.row, currPos.col].GetComponent<PipeButton>().up && pipes[currPos.row+1, currPos.col].GetComponent<PipeButton>().down)
            {
                toVisit.Push((currPos.row+1, currPos.col));
            }
            
            // Check down
            if (currPos.row-1 >= 0 && !visited.Contains((currPos.row-1, currPos.col)) && pipes[currPos.row, currPos.col].GetComponent<PipeButton>().down && pipes[currPos.row-1, currPos.col].GetComponent<PipeButton>().up)
            {
                toVisit.Push((currPos.row-1, currPos.col));
            }

            // Check left
            if (currPos.col-1 >= 0 && !visited.Contains((currPos.row, currPos.col-1)) && pipes[currPos.row, currPos.col].GetComponent<PipeButton>().left && pipes[currPos.row, currPos.col-1].GetComponent<PipeButton>().right)
            {
                toVisit.Push((currPos.row, currPos.col-1));
            }

            // Check right
            if (currPos.col+1 < nCols && !visited.Contains((currPos.row, currPos.col+1)) && pipes[currPos.row, currPos.col].GetComponent<PipeButton>().right && pipes[currPos.row, currPos.col+1].GetComponent<PipeButton>().left)
            {
                toVisit.Push((currPos.row, currPos.col+1));
            }
        }

        return false;
    }

}
