using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster checkpoint;
    public Vector2 lastCheckPointPosition;

    void Awake()
    {
        if (checkpoint == null)
        {
            checkpoint = this;
            DontDestroyOnLoad(checkpoint);
        }
        else
            Destroy(gameObject);
    }
}