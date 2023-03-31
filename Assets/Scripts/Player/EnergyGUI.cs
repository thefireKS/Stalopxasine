using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGUI : MonoBehaviour
{
    [SerializeField]
    private Image bar;

    private void OnEnable() => UltimateEnergy.OnEnergyChanged += GetCurrentFill;
    
    private void OnDisable() => UltimateEnergy.OnEnergyChanged -= GetCurrentFill;
    void GetCurrentFill(int energy, int fullEnergy)
    {
        float fillAmount = (float) energy / (float) fullEnergy;
        bar.fillAmount = fillAmount;
    }
}