using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    private LevelData _levelData;

    public void SetLevelDataAndLoad(LevelData levelData)
    {
        _levelData = levelData;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(_levelData.scene);
    }

    public LevelData ReturnLevelData()
    {
        return _levelData;
    }
}