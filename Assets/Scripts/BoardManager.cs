using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private List<List<string>> boards;
    private List<List<GameObject>> grids;
    private List<List<Element>> gridUsing;
    private int ySize;
    private int xSize;
    private List<Element> elements;
    private GameObject gridPrefab;
    private GameObject elementPrefab;
    void Start()
    {
        gridPrefab = Resources.Load("Prefabs/Grid") as GameObject;
        elementPrefab = Resources.Load("Prefabs/Element") as GameObject;
    }
    #region Implementation
    private void ResetCurrBoard()
    {
        grids = new List<List<GameObject>>();
        elements = new List<Element>();
        gridUsing = new List<List<Element>>();
    }
    #endregion
    #region Interface
    public void SetLevelBoard(LevelConfig levelConfig)
    {
        ResetCurrBoard();
        boards = levelConfig.Boards;
        var BoardsObject = new GameObject();
        BoardsObject.name = "Boards";
        ySize = boards.Count;
        xSize = boards[0].Count;

        for(var y = 0; y < ySize; ++y)
        {
            List<Element> gridUsingCol = new List<Element>();
            for(var x = 0; x < xSize; ++x)
            {
                gridUsingCol.Add(null);
            }
            gridUsing.Add(gridUsingCol);
        }

        var xBegin = (Camera.main.ScreenToWorldPoint(new Vector2(GameManager.Instance.ScreenWidth / 2, 0)).x - xSize) / 2;
        var yBegin = (Camera.main.ScreenToWorldPoint(new Vector2(0, GameManager.Instance.ScreenHeight / 2)).y + ySize) / 2;
        for(var y = 0; y < ySize; ++y)
        {
            List<GameObject> gridCol = new List<GameObject>();
            for(var x = 0; x < xSize; ++x)
            {
                var gridObj = Instantiate(gridPrefab, new Vector2(xBegin + x, yBegin - y), Quaternion.identity, BoardsObject.transform);
                gridObj.name = "Grid" + "X" + x + "Y" + y;
                gridCol.Add(gridObj);

                if (!string.IsNullOrWhiteSpace(boards[y][x]))
                {
                    var elementObj = Instantiate(elementPrefab, new Vector2(xBegin + x, yBegin - y), Quaternion.identity, BoardsObject.transform);
                    elementObj.name = "Element" + "X" + x + "Y" + y;
                    var element = elementObj.AddComponent<Element>();
                    element.InitElementFromText(boards[y][x]);
                    if (!element.shouldRemove)
                    {
                        element.InitPosition(x, y);
                        elements.Add(element);
                        gridUsing[y][x] = element;
                    }
                }
            }
            grids.Add(gridCol);
        }
        canMove = true;
    }
    public void RemoveElement(Element element)
    {
        gridUsing[element.positionInGrid.yPos][element.positionInGrid.xPos] = null;
        elements.Remove(element);
    }
    #region Move
    bool canMove;
    public void Move(Vector2 direction)
    {
        canMove = false;
        int x = (int)direction.x;
        int y = (int)-direction.y;
        int xBegin, xEnd, yBegin, yEnd, xChange, yChange;
        if(x == 0)
        {
            xBegin = 0;
            xEnd = xSize;
            xChange = 1;
            if(y > 0)
            {
                y = 1;
                yBegin = ySize - 1;
                yEnd = -1;
                yChange = -1;
            }
            else
            {
                y = -1;
                yBegin = 0;
                yEnd = ySize;
                yChange = 1;
            }
        }
        else
        {
            y = 0;
            yBegin = 0;
            yEnd = ySize;
            yChange = 1;
            if(x > 0)
            {
                x = 1;
                xBegin = xSize - 1;
                xEnd = -1;
                xChange = -1;
            }
            else
            {
                x = -1;
                xBegin = 0;
                xEnd = xSize;
                xChange = 1;
            }
        }
        for (var i = yBegin; i != yEnd; i += yChange)
        {
            for (var j = xBegin; j != xEnd ; j += xChange)
            {
                if (gridUsing[i][j])
                {
                    if (0 <= i + y && i + y < ySize && 0 <= j + x && j + x < xSize && gridUsing[i + y][j + x] == null)
                    {
                        var element = gridUsing[i][j];
                        Debug.Log("i: " + i + "j: " + j);
                        if (element.type == Element.ElementType.normal)
                        {
                            element.positionInGrid.UpdatePosition(x, y);
                            element.transform.position += (Vector3)direction;
                            gridUsing[i + y][j + x] = element;
                            gridUsing[i][j] = null;
                        }
                    }
                }
            }
        }
        canMove = true;
    }
    #endregion
    private float lastMoveT = 0f, moveDelay = .3f;
    void Update()
    {
        if(lastMoveT <= moveDelay)
        {
            lastMoveT += Time.deltaTime;
        }
        else if (canMove)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if ((x != 0 || y != 0) && !(x != 0 && y != 0))
            {
                lastMoveT = 0f;
                Move(new Vector2(x, y));
            }
        }
    }
    #endregion
}
