using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaramelloHolyAura : MonoBehaviour
{
    [SerializeField] private GameObject tipCanvas;
    [SerializeField] private GameObject mouseTrail;
    private Animator anim;
    private UltimateEnergy ue;

    private float width = Screen.width;
    private bool nextLeft;
    private int ultimateSize = 0;

    private GameObject cachedCanvas;
    private GameObject cachedMouseTrail;

    public static event Action ChangeSide;
    private void Start()
    {
        ue = GetComponentInParent<UltimateEnergy>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        cachedCanvas = Instantiate(tipCanvas);
        cachedMouseTrail = Instantiate(mouseTrail);
    }

    private void Update()
    {
        if (ultimateSize > 7)
            ue.canEndEarlier = true;
        
        anim.SetInteger("ultimateSize",ultimateSize);
        RightLeftEvent();
    }

    private void RightLeftEvent()
    {
        if (Input.mousePosition.x > (width / 4 * 3))
        {
            if (Input.GetMouseButton(0) && !nextLeft)
            {
                ultimateSize++;
                nextLeft = !nextLeft;
                ChangeSide?.Invoke();
            }
        }
        if (Input.mousePosition.x < (width / 4))
        {
            if (Input.GetMouseButton(0) && nextLeft)
            {
                nextLeft = !nextLeft;
                ChangeSide?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        Destroy(cachedCanvas);
        Destroy(cachedMouseTrail);
    }
}