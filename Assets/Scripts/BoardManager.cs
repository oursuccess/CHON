using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private List<List<string>> boards;
    private List<List<GameObject>> grids;
    private List<List<Element>> gridUsing;
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
        int ySize = boards.Count;
        int xSize = boards[0].Count;

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
                        element.OnPositionUpdated += OnGridPositionChanged;
                        elements.Add(element);
                        gridUsing[y][x] = element;
                    }
                }
            }
            grids.Add(gridCol);

            canMove = true;
        }
    }
    public void RemoveElement(Element element)
    {
        gridUsing[element.positionInGrid.yPos][element.positionInGrid.xPos] = null;
        element.OnPositionUpdated -= OnGridPositionChanged;
        elements.Remove(element);
    }
    #region Move
    bool canMove;
    public void Move(Vector2 direction)
    {
        canMove = false;
        for(var i = 0; i < gridUsing.Count; ++i)
        {
            for(var j = 0; j < gridUsing[0].Count; ++j)
            {
                gridUsing[i][j]?.Move(direction);
            }
        }
        canMove = true;
    }
    private void OnGridPositionChanged(Element element)
    {
        gridUsing[element.positionInGrid.yPos][element.positionInGrid.xPos] = element;
    }
    #endregion
    void Update()
    {
        if (canMove)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if ((x != 0 || y != 0) && !(x != 0 && y != 0))
            {
                Move(new Vector2(x, y));
            }
        }
    }
    #endregion
}
