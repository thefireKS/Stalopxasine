using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject Ficus;
    void Update()
    {
        if (Input.GetKey("f"))
        {
            Instantiate(Ficus);
        }
    }
}
