using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelLock : MonoBehaviour
{
    [SerializeField] 
    private BoxCollider2D exitCollider;
    [SerializeField] 
    private int enemiesToDefeat;
    [SerializeField] 
    private TextMeshProUGUI dataText;

    public static int killedEnemies;

    private void Start()
    {
        killedEnemies = 0;
        exitCollider.enabled = false;
    }

    private void Update()
    {
        if (killedEnemies >= enemiesToDefeat)
            exitCollider.enabled = true;
        else
            exitCollider.enabled = false;
        dataText.text = killedEnemies.ToString() + "/" + enemiesToDefeat.ToString();
    }
}
