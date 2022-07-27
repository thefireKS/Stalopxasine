using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public GameObject[] objects;

    public void Awake()
    {
        Globals.CreatedCharacter = Instantiate(objects[Globals.Character - 1],new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
    }
}