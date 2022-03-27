using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    Transform player;
    private GameMaster gamemaster;
    public Transform FirstLevelPosition;
    
    public int EndingNumber;

    private void Start()
    {
        player = Globals.CreatedCharacter.GetComponentInChildren<Transform>();
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Globals.StartEnding = true;
            gamemaster.lastCheckPointPosition = FirstLevelPosition.position;
            Globals.CharPositions[Globals.Character-1] = FirstLevelPosition.position;
            Destroy(collision.gameObject);
        }
    }
}
