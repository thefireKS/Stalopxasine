using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeString : MonoBehaviour
{
    [SerializeField] private LocalizationSystem system;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private string english;
    [SerializeField] private string russian;
  
    private void OnEnable() => system.OnLanguageChanged += LocalizeUI;
    private void OnDisable() => system.OnLanguageChanged -= LocalizeUI;

    private void Start()
    {
        LocalizeUI(system.CurrentLanguage);
    }

    private void LocalizeUI(Languages currentLanguage)
    {
        switch (currentLanguage)
        {
            case Languages.English:
                tmp.text = english;
                break;
            case Languages.Russian:
                tmp.text = russian;
                break;
        }
    }
}