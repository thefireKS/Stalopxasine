using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionUI : MonoBehaviour
{
    public CharacterSelectionData data;
    private LevelSelection[] selections;

    // TODO: снести нахуй
    public void Awake()
    {
        selections = GetComponentsInChildren<LevelSelection>();
        for (int index = 0; index < data.characters.Length; index++)
        {
            //selections[index].Setup(data.characters[index],data);
        }
    }
}