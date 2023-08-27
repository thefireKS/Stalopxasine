using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGUI : MonoBehaviour
{
    [SerializeField]
    private Image bar;

    private void OnEnable() => PlayerUltimateSystem.OnEnergyChanged += GetCurrentFill;
    
    private void OnDisable() => PlayerUltimateSystem.OnEnergyChanged -= GetCurrentFill;
    void GetCurrentFill(int energy, int fullEnergy)
    {
        float fillAmount = (float) energy / (float) fullEnergy;
        bar.fillAmount = fillAmount;
    }
}