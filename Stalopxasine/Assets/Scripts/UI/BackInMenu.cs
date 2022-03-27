using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackInMenu : MonoBehaviour
{
    public void Backing()
    {
        Globals.Character = 0;
        SceneManager.LoadScene("Character Selection");
    }
}
