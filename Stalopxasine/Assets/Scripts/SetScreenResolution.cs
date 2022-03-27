using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreenResolution : MonoBehaviour
{
    void Awake()
    {
        Screen.SetResolution(960,540,false);
    }
}
