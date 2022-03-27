﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cam;

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
    } 
}
