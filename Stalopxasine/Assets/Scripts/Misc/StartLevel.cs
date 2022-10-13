using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartLevel : MonoBehaviour
{
    public CharacterSelectionData data;

    public void Awake()
    {
        data.spawnedCharacter = data.selectedCharacter.Spawn(transform);
    }
}