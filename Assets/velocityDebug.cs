using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class velocityDebug : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        
        text.text = "Velocity: " + _rigidbody2D.velocity;
    }
}
