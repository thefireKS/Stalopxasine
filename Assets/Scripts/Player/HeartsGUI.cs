﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsGUI : MonoBehaviour
{
    public int NumberOfHearts;

    //CharacterHP characterHP;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private void OnEnable() => CharacterHP.OnHealthChanged += UpdateHearts;
    private void OnDisable() => CharacterHP.OnHealthChanged -= UpdateHearts;
    private void UpdateHearts(int health)
    {
        if (health > NumberOfHearts)
            health = NumberOfHearts;
        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
            if (i < NumberOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }
}