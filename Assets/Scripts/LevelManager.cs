using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        InitLevels();
        InitBoardManager();
    }
    #region Scene
    public void ToSceneSelector()
    {
        SceneManager.LoadScene(LevelSelectorName);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
    #region Level
    public void LoadLevel(int level)
    {
    }
    #region Init
    #region Levels
    private void InitLevels()
    {
        Levels = Resources.LoadAll<TextAsset>(LevelDictionaryName);
#if UNITY_EDITOR
        foreach(var level in Levels)
        {
            if(!int.TryParse(level.name, out int levelNum))
            {
                Debug.Log("发现了不支持的关卡文件\n" + level.name);
            }
        }
#endif
        FinalLevel = Levels.Length;
    }
    public int FinalLevel { get; private set; }
    public TextAsset[] Levels { get; private set; }
    #endregion
    #region BoardManager
    public BoardManager BoardManager { get; private set; }
    public void InitBoardManager()
    {
        BoardManager = gameObject.AddComponent<BoardManager>();
    }
    #endregion
    #endregion
    #region KeepInDate
    #region SceneName
    //Scene名称变化或增加后需要更新
    public static string StartUPSceneName { get; } = "StartUP";
    public static string LevelSelectorName { get; } = "LevelSelector";
    public static string MainSceneName { get; } = "Main";
    #endregion
    #region LevelDict
    //若变更了Level存储位置，则需要更新，目前使用Resources.Load，故只存储了文件夹名称，而无路径
    public static string LevelDictionaryName { get; } = "Levels";
    #endregion
    #endregion
    #endregion
}
