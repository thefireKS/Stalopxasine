using UnityEngine;

[CreateAssetMenu]
public class CharacterSelectionData: ScriptableObject
{
    public GameObject spawnedCharacter;
    public PlayerData selectedCharacter;
    public PlayerData[] characters;
}