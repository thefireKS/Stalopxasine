using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private Image showing;
    private PlayerData data;
    private CharacterSelectionData selectionData;

    public void LoadScene()
    {
        selectionData.selectedCharacter = data;
        SceneManager.LoadScene(data.sceneName);
    }

    public void Setup(PlayerData playerData, CharacterSelectionData csData)
    {
        data = playerData;
        selectionData = csData;
        GetComponent<Image>().color = data.color;
        showing.sprite = data.characterSprite;
    }
}