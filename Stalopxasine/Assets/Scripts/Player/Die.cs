﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private CharacterHP characterHP;
    private void Start()
    {
        characterHP = Globals.CreatedCharacter.GetComponentInChildren<CharacterHP>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            characterHP.HP = 0;
        
        if (other.CompareTag("Enemy"))
            Destroy(other.gameObject);
    }
}