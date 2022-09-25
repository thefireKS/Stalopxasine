using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    private Transform player;
    private Transform actualPlayerPosition;
    public Transform newLevelPosition;
    private GameMaster gamemaster;
    private CharacterHP charHP;

    private void Start()
    {
        player = Globals.CreatedCharacter.GetComponent<Transform>();
        foreach (Transform child in player)
        {
            if (child.tag == "Player")
                actualPlayerPosition = child.GetComponent<Transform>();
        }
        Debug.Log(actualPlayerPosition.name);
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        charHP = Globals.CreatedCharacter.GetComponentInChildren<CharacterHP>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           gamemaster.lastCheckPointPosition = newLevelPosition.position;
           
           player.position = newLevelPosition.position;
           actualPlayerPosition.position = player.position;
           Globals.CharPositions[Globals.Character-1] = newLevelPosition.position;
           
           charHP.HP = charHP.FullHP;
           LevelLock.killedEnemies = 0;
           
        }
    }
}