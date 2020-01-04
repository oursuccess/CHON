using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    private void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    #region Mono
    void Awake()
    {
        Singleton();
        InitLevelManager();
    }
    void Start()
    {
    }
    void Update()
    {

    }
    #endregion
    #region LevelManager
    public LevelManager LevelManager { get; private set; }
    private void InitLevelManager()
    {
        LevelManager = gameObject.AddComponent<LevelManager>();
    }
    #region Interface
    public void LoadScene(string levelName)
    {
        LevelManager.LoadScene(levelName);
    }
    public void ExitGame()
    {
        LevelManager.ExitGame();
    }
    #endregion
    #endregion
    #region GameState
    #region GameOver
    public void GameOver()
    {
        AskIfRetry();
    }
    private void AskIfRetry()
    {

    }
    #endregion
    #endregion
    #region SceneName
    //Scene更新后需要更新
    public static string StartUPSceneName = "StartUP";
    public static string MainSceneName = "Main";
    #endregion
}
