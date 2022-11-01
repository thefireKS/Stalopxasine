using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireKSCanvasHitMaker : MonoBehaviour
{
    [SerializeField] private GameObject upHit;
    [SerializeField] private GameObject downHit;

    private void OnEnable()
    {
        fireKSWall.ChangeSide += ChangingSide;
    }
    
    private void ChangingSide()
    {
        if (upHit.activeSelf)
        {
            downHit.SetActive(true);
            upHit.SetActive(false);
        }
        else
        {
            upHit.SetActive(true);
            downHit.SetActive(false);
        }
            
    }

    private void OnDisable()
    {
        fireKSWall.ChangeSide -= ChangingSide;
    }
}