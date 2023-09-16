using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public LevelData _levelData;
    private static LevelSelection instance;
    public void SetLevelDataAndLoad(LevelData levelData)
    {
        _levelData = levelData;
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            instance._levelData = levelData;
        }
        else
        {
            instance._levelData = levelData;
            Destroy(this);
        }
        SceneManager.LoadScene(_levelData.scene);
    }

    public LevelData ReturnLevelData()
    {
        return _levelData;
    }
}