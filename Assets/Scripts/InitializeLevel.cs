using Player;
using UnityEngine;

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
        var levelData = FindObjectOfType<LevelSelection>().ReturnLevelData();
        Debug.Log("Level Initialization: Find LevelData");
        playerInitializer.Initialize(levelData.playerData);
        Debug.Log("Level Initialization: Complete");
    }
}
