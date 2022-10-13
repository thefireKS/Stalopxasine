using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class CharacterSelectionData: ScriptableObject
{
    public GameObject spawnedCharacter;
    public PlayerData selectedCharacter;
    public PlayerData[] characters;
}