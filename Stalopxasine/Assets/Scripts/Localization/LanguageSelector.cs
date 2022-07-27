using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private Languages language;
    [SerializeField] private LocalizationSystem system;  

    public void ChangeLanguage()
    {
        system.ChangeLanguage(language);
    }
}
