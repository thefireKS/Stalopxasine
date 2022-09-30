using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerMeeting : MonoBehaviour
{
    private Animator anim;
    private bool inZone=false;
    public static bool DialogIsGoing=false;

    public GameObject Ebutton;
    public GameObject DialogWindow;
    public GameObject FirstTextWindow;
    
    private Camera cam;
    private float memcam;

    [SerializeField] 
    private string afkAnimName;

    [SerializeField] 
    private string GreetingAnimName;

    private void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
        memcam = cam.orthographicSize;
    }

    private void CameraZoom()
    {
        if(cam.orthographicSize == memcam)
            cam.orthographicSize = cam.orthographicSize / 2;
        else
            cam.orthographicSize = cam.orthographicSize * 2;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Ebutton.SetActive(true);
            inZone = true;
        }
    }

    public void DialogEnd()
    {
        DialogIsGoing = false;
        Ebutton.SetActive(true);
        CameraZoom();
    }

    private void Update()
    {
        if (Input.GetKeyDown("e") && inZone && !DialogIsGoing)
        {
            DialogIsGoing = true;
            anim.Play(GreetingAnimName);
            Ebutton.SetActive(false);
            DialogWindow.SetActive(true);
            FirstTextWindow.SetActive(true);
            CameraZoom();
        }
        if(inZone && !DialogIsGoing)
            anim.Play(afkAnimName);
        
        Debug.Log(DialogIsGoing);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            Ebutton.SetActive(false);
            anim.Play(afkAnimName);
        }
    }
}