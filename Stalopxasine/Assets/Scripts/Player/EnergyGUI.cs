using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGUI : MonoBehaviour
{
    [SerializeField]
    private Image bar;

    private UltimateEnergy ue;
    
    void Start()
    {
        ue = Globals.CreatedCharacter.GetComponentInChildren<UltimateEnergy>();
    }
    void Update()
    {
        GetCurrentFill();
    }
    
    void GetCurrentFill()
    {
        float fillAmount = (float) ue.Energy / (float) ue.FullEnergy;
        bar.fillAmount = fillAmount;
    }
}