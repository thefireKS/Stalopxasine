using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    private Transform player;
    public Transform newLevelPosition;
    private GameMaster gamemaster;
    private CharacterHP charHP;

    private void Start()
    {
        player = Globals.CreatedCharacter.GetComponent<Transform>();
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        charHP = Globals.CreatedCharacter.GetComponent<CharacterHP>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           gamemaster.lastCheckPointPosition = newLevelPosition.position;
           
           player.position = newLevelPosition.position;
           Globals.CharPositions[Globals.Character-1] = newLevelPosition.position;
           charHP.HP = charHP.FullHP;
           LevelLock.killedEnemies = 0;
           
        }
    }
}