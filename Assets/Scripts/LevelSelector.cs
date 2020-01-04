using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    #region Canvas
    [SerializeField]
    [Tooltip("包含场景背景图和按钮的canvas\n命名：ImageBackGround：背景图\nButtonLevel：场景选择")]
    private Canvas LevelSelectorCanvas;
    [SerializeField]
    [Tooltip("关卡选择的按钮")]
    private Button LevelSelectButton;
    #endregion
    #region GridCount
    [System.Serializable]
    struct GridSize
    {
        [Tooltip("行数量")]
        public int col;
        [Tooltip("列数量")]
        public int row;
    }
    [SerializeField]
    [Tooltip("关卡选择界面的横纵格子数量")]
    private GridSize gridSize;
    #endregion
    #region Var
    private Canvas canvas;
    private int currPage;
    private List<List<Button>> levelButtons;
    #endregion
    void Start()
    {
        canvas = Instantiate(LevelSelectorCanvas);
        canvas.name = LevelSelectorCanvas.name;

        InitPage();
    }
    #region InitPage
    private void InitPage()
    {
        InitLevelButtons();
    }
    #endregion
    #region ToPage
    private void ToPage(int Page)
    {
        if(currPage != Page)
        {
            currPage = Page;
            HandleCurrLevelButtons();
            SetLevelButtons(Page);
        }
    }
    private void ToNextPage()
    {
        if (currPage + 1 <= GameManager.Instance.LevelNum / gridSize.col / gridSize.row + 1)
        {
            ToPage(currPage + 1);
        }
    }
    private void ToPrevPage()
    {
        if(currPage - 1 > 0)
        {
            ToPage(currPage - 1);
        }
    }
    #endregion
    private void InitLevelButtons()
    {
        levelButtons = new List<List<Button>>();
        //进行关卡按钮的数量、位置调整，并将按钮与关卡切换函数绑定
        for (int i = 0, y = 0; y < gridSize.row; ++y)
        {
            List<Button> ButtonCols = new List<Button>();
            for (var x = 0; x < gridSize.col; ++x)
            {
                ++i;
                if (LastLevelFinished(i)) break;
                var button = InitLevelButton(i);
                InitLevelButtonPos(button, x, y);
                ButtonCols.Add(button);
            }
            if (LastLevelFinished(i)) break;
            levelButtons.Add(ButtonCols);
        }
    }
    private bool LastLevelFinished(int levelNum)
    {
        return levelNum > GameManager.Instance.LevelNum;
    }
    private Button InitLevelButton(int buttonNum)
    {
        var button = Instantiate(LevelSelectButton, canvas.transform);
        button.transform.Find("Text").GetComponent<Text>().text = buttonNum.ToString().PadLeft(3, '0');
        button.onClick.AddListener(OnLevelSelectButtonClicked);
        return button;
    }
    private void InitLevelButtonPos(Button button, int x, int y)
    {
        var rect = button.gameObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);


        float xPos = x * GameManager.Instance.ScreenWidth / gridSize.col;
        float yPos = -y * GameManager.Instance.ScreenHeight / gridSize.row;
        rect.anchoredPosition = new Vector2(xPos, yPos);
    }
    private void HandleCurrLevelButtons()
    {
        foreach(var buttonCol in levelButtons)
        {
            foreach(var button in buttonCol)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
    private void SetLevelButtons(int Page)
    {
        for(int i = Page * gridSize.col * gridSize.row, y = 0;  y < gridSize.row; ++y)
        {
            for(var x = 0; x < gridSize.col; ++x)
            {
                ++i;
                if(LastLevelFinished(i)) break;
                var button = levelButtons[x][y];
                button.gameObject.SetActive(true);
                button.transform.Find("Text").GetComponent<Text>().text = i.ToString().PadLeft(3, '0');
            }
            if (LastLevelFinished(i)) break;
        }
    }
    private void OnLevelSelectButtonClicked()
    {
        var currButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        var levelText = currButton.transform.Find("Text").GetComponent<Text>();
#if UNITY_EDITOR
        if(!int.TryParse(levelText.text, out int levelNum))
        {
            Debug.Log("读取当前关卡按钮的内容出错\n" + levelText);
        }
#endif
        GameManager.LoadLevel(int.Parse(levelText.text));
    }
}
