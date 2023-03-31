using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamMoving : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private bool Created;
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        if(!Created)
            player = GameObject.FindGameObjectWithTag("Player");

        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = player.transform;
    }
}