using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    public CharacterSelectionData data;
    private CharacterHP characterHP;
    private void Awake()
    {
        characterHP = data.spawnedCharacter.GetComponent<CharacterHP>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            characterHP.HP = 0;
        
        if (other.CompareTag("Enemy"))
            Destroy(other.gameObject);
    }
}
