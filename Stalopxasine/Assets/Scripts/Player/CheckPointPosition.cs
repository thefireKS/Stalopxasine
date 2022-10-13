using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointPosition : MonoBehaviour
{
    private GameMaster gamemaster;
    public CharacterSelectionData data;

    void Start()
    {
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        if (Globals.CharPositions.ContainsKey(data.selectedCharacter))
            data.spawnedCharacter.transform.position = Globals.CharPositions[data.selectedCharacter];
    }
}
