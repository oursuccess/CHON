using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private List<List<string>> boards;
    private List<List<GameObject>> grids;
    private List<List<GameObject>> elements;
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
        elements = new List<List<GameObject>>();
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

        var xBegin = (Camera.main.ScreenToWorldPoint(new Vector2(GameManager.Instance.ScreenWidth / 2, 0)).x - xSize) / 2;
        var yBegin = (Camera.main.ScreenToWorldPoint(new Vector2(0, GameManager.Instance.ScreenHeight / 2)).y + ySize) / 2;
        for(var y = 0; y < ySize; ++y)
        {
            List<GameObject> gridCol = new List<GameObject>();
            List<GameObject> elementCol = new List<GameObject>();
            for(var x = 0; x < xSize; ++x)
            {
                var grid = Instantiate(gridPrefab, new Vector2(xBegin + x, yBegin - y), Quaternion.identity, BoardsObject.transform);
                grid.name = "Grid" + "X" + x + "Y" + y;
                gridCol.Add(grid);

                var element = Instantiate(elementPrefab, new Vector2(xBegin + x, yBegin - y), Quaternion.identity, BoardsObject.transform);
                element.name = "Element" + "X" + x + "Y" + y;
                var elementText = element.transform.Find("Text").gameObject.GetComponent<TextMesh>();
                elementText.text = boards[y][x];
                elementCol.Add(element);
            }
            grids.Add(gridCol);
        }
    }
    #endregion
}
