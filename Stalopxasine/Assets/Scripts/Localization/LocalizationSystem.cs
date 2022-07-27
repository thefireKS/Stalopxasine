using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
  English,
  Russian,
}

[CreateAssetMenu]
public class LocalizationSystem : ScriptableObject
{
  [SerializeField] private Languages currentLanguage = Languages.English;
  public Languages CurrentLanguage 
  {
    get => currentLanguage;
    set => currentLanguage = value;
  }
  public System.Action<Languages> OnLanguageChanged;

  public void ChangeLanguage(Languages newLanguage)
  {
    CurrentLanguage = newLanguage;
    OnLanguageChanged?.Invoke(currentLanguage);
  }
}