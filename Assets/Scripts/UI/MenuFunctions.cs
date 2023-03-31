using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void Backing()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Character Selection");
    }
    public void Continuing(GameObject pauseMenu)
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
