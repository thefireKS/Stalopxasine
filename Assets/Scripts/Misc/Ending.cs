using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public CharacterSelectionData data;
    private GameMaster gamemaster;
    public Transform FirstLevelPosition;

    private void Start()
    {
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Globals.StartEnding = true;
            gamemaster.lastCheckPointPosition = FirstLevelPosition.position;
            Globals.CharPositions[data.selectedCharacter] = FirstLevelPosition.position;
            Destroy(collision.gameObject);
        }
    }
}
