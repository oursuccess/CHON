using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUP : MonoBehaviour
{
    [SerializeField]
    [Tooltip("包含开始界面的Canvas，\n应包括：命名为ImageBackGround的背景图，以及以Button开头的按钮，\n且按钮至少包括：ButtonStart，ButtonExit二者，每次更新时需要重写StarUP")]
    public Canvas StartUPCanvas;

    void Start()
    {
        var canvas = Instantiate(StartUPCanvas);
        //进行按钮位置的调整

        Button StartButton = canvas.transform.Find("ButtonStart").gameObject.GetComponent<Button>();
        StartButton.onClick.AddListener(OnStartButtonClicked);
        Button ExitButton = canvas.transform.Find("ButtonExit").gameObject.GetComponent<Button>();
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        GameManager.Instance.LoadScene(GameManager.MainSceneName);
    }
    private void OnExitButtonClicked()
    {
        GameManager.Instance.ExitGame();
    }
}
