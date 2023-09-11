using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    //public CharacterSelectionData data;
    private Transform player;
    public Transform newLevelPosition;
    private GameMaster gamemaster;

    public static event Action SetMaxHealth;

    private void Start()
    {
        //player = data.spawnedCharacter.GetComponent<Transform>();
        gamemaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            SetMaxHealth?.Invoke();
           gamemaster.lastCheckPointPosition = newLevelPosition.position;
           player.position = newLevelPosition.position;
          // Globals.CharPositions[data.selectedCharacter] = newLevelPosition.position;
           LevelLock.killedEnemies = 0;
           
        }
    }
}