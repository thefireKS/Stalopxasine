using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class fireKSWall : MonoBehaviour
{
    [SerializeField] private GameObject tipCanvas;
    [SerializeField] private GameObject mouseTrail;
    [SerializeField] private GameObject ultimateWall;
    [SerializeField] private SpriteRenderer sr;
    
    private PlayerUltimateSystem ue;

    private float height = Screen.height;
    private float width = Screen.width;
    private bool nextDown;
    private bool isFacingLeft;
    private int ultimateSize = 0;

    private GameObject cachedCanvas;
    private GameObject cachedMouseTrail;
    private GameObject cachedUltimateWall;

    [SerializeField ]private Transform parentPosition;

    public static event Action ChangeSide;
    public static event Action<int> ChangeScale;
    public static event Action FlipObject;
    public static event Action LaunchWall;
    private void Start()
    {
        //parentPosition = GetComponentInParent<Transform>();
        ue = GetComponentInParent<PlayerUltimateSystem>();
    }
    private void OnEnable()
    {
        sr.flipX = false;
        cachedCanvas = Instantiate(tipCanvas);
        cachedMouseTrail = Instantiate(mouseTrail);
        cachedUltimateWall = Instantiate(ultimateWall, parentPosition.position + new Vector3(1,0,0), quaternion.identity);
    }
    private void Update()
    {
        if (ultimateSize > 7)
            ue.canEndEarlier = true;
        UpDownEvent();
    }
    private void UpDownEvent()
    {
        if (Input.mousePosition.y > (height / 4 * 3))
        {
            if (Input.GetMouseButton(0) && !nextDown)
            {
                ultimateSize++;
                nextDown = !nextDown;
                ChangeScale?.Invoke(ultimateSize);
                ChangeSide?.Invoke();
            }
        }
        if (Input.mousePosition.y < (height / 4))
        {
            if (Input.GetMouseButton(0) && nextDown)
            {
                nextDown = !nextDown;
                ChangeSide?.Invoke();
            }
        }

        if ((Input.mousePosition.x > width / 2 && isFacingLeft ) || (Input.mousePosition.x < width / 2 && !isFacingLeft))
        {
            sr.flipX = !sr.flipX;
            FlipObject?.Invoke();
            isFacingLeft = !isFacingLeft;
        }
    }
    
    private void OnDisable()
    {
        LaunchWall?.Invoke();
        Destroy(cachedCanvas);
        Destroy(cachedMouseTrail);
    }
}