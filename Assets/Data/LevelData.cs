using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level/Data")]
public class LevelData : ScriptableObject
{
    // TODO: add difficulty
    public string scene;

    public PlayerData playerData;
}
