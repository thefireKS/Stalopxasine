using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public string SceneName;
    public int NumberCharacter; //1-Caramello, 2-fireKS, 3-Fridman, 4-Viseman
    public void LoadScene()
    {
        Globals.Character = NumberCharacter;
        SceneManager.LoadScene(SceneName);
    }
}
