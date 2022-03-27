using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsGUI : MonoBehaviour
{
    int health;
    public int NumberOfHearts;

    CharacterHP characterHP;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Start()
    {
        characterHP = Globals.CreatedCharacter.GetComponentInChildren<CharacterHP>();
    }
    private void Update()
    {
        health = characterHP.HP;
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
