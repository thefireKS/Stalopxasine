using Player;
using UnityEngine;
using UnityEngine.Localization.Components;

public class InitializeLevel : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }

    private static void Initialize()
    {
        var playerInitializer = FindObjectOfType<InitializePlayer>();
        Debug.Log("Level Initialization: Find InitializePlayer");
        var levelSelection = FindObjectOfType<LevelSelection>();
        LevelData levelData = null;
        if (levelSelection )
        {
            levelData = levelSelection.ReturnLevelData();
            Debug.Log("Level Initialization: Find LevelData");
            playerInitializer.Initialize(levelData.playerData);
            Debug.Log("Level Initialization: Complete");
            GameObject.Find("Character tips").GetComponent<LocalizeStringEvent>().StringReference =
                levelData.playerData.uiDescription;
        }
        else
        {
            Debug.LogWarning("Level Initialization: Can't find LevelData");
        }
        Time.timeScale = 1f;
    }
}
