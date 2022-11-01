using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaramelloCanvasHitMaker : MonoBehaviour
{
    [SerializeField] private GameObject rightHit;
    [SerializeField] private GameObject leftHit;
    private void OnEnable()
    {
        CaramelloHolyAura.ChangeSide += ChangingSide;
    }

    private void ChangingSide()
    {
        if (rightHit.activeSelf)
        {
            leftHit.SetActive(true);
            rightHit.SetActive(false);
        }
        else
        {
            rightHit.SetActive(true);
            leftHit.SetActive(false);
        }
            
    }

    private void OnDisable()
    {
        CaramelloHolyAura.ChangeSide -= ChangingSide;
    }
}
