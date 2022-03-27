using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointPosition : MonoBehaviour
{
    private GameMaster gamemaster;

    void Start()
    {
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        if (Globals.CharPositions.ContainsKey(Globals.Character-1))
            Globals.CreatedCharacter.transform.position = Globals.CharPositions[Globals.Character - 1];
    }
}
