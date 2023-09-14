using UnityEngine;

public class CheckPointPosition : MonoBehaviour
{
    private GameMaster _gameMaster;
    public CharacterSelectionData data;

    void Start()
    {
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        if (Globals.CharPositions.ContainsKey(data.selectedCharacter))
            data.spawnedCharacter.transform.position = Globals.CharPositions[data.selectedCharacter];
    }
}
