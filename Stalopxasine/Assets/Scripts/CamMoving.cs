using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamMoving : MonoBehaviour
{
    public GameObject player;
    public bool Created = true;
    private CinemachineVirtualCamera cinemachineVirtualCamera; 

    private void Start()
    {
        if(Created)
            player = Globals.CreatedCharacter;

        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = player.transform;
        cinemachineVirtualCamera.LookAt = player.transform;
    }
}