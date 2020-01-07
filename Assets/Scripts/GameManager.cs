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
    void Awake()
    {
        Singleton();
        InitLevelManager();
    }
    void Start()
    {
    }
    #region LevelManager
    #region Implematetion
    private LevelManager LevelManager;
    private void InitLevelManager()
    {
        LevelManager = gameObject.AddComponent<LevelManager>();
    }
    #endregion
    #region Interface
    public int LevelNum
    {
        get
        {
            return LevelManager.FinalLevel;
        }
    }
    public static void LoadLevel(int levelNum)
    {
        Instance.LevelManager.LoadLevel(levelNum);
    }
    #endregion
    #endregion
    #region Camera
    public float ScreenWidth { get { return Screen.width; } }
    public float ScreenHeight { get { return Screen.height; } }
    #endregion
    #region Scene 
    public static void GameStart()
    {
        Instance.LevelManager.ToSceneSelector();
    }
    public static void ExitGame()
    {
        Instance.LevelManager.ExitGame();
    }
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
}
