using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGUI : MonoBehaviour
{
    public CharacterSelectionData data;
    
    [SerializeField]
    private Image bar;

    private UltimateEnergy ue;
    
    void Start()
    {
        ue = data.spawnedCharacter.GetComponent<UltimateEnergy>();
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