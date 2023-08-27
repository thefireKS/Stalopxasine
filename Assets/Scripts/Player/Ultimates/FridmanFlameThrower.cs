using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridmanFlameThrower : MonoBehaviour
{
    public int possibleAttacks = 6;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject flame;
    
    private PlayerUltimateSystem ue;
    private int currentAttacks = 0;
    private float attacksSpace = 0f;

    private float oldCamSize;

    private void Start()
    {
        ue = GetComponentInParent<PlayerUltimateSystem>();
    }

    private void OnEnable()
    {
        oldCamSize = Camera.main.orthographicSize;
        Camera.main.orthographicSize *= 1.25f;
    }

    private void Update()
    {
        SetPosition();

        if (Input.GetMouseButton(0) && currentAttacks < possibleAttacks && attacksSpace > 0.33f)
        {
            Debug.Log(currentAttacks);
            attacksSpace = 0f;
            var _flame = Instantiate(flame, attackPoint.position, Quaternion.Euler(0,0,attackPoint.rotation.eulerAngles.z + 45f));
            _flame.GetComponentInChildren<Animator>().SetFloat("Angle", _flame.transform.eulerAngles.z % 10 == 0 ? 0 : 1);
            currentAttacks++;
        }

        if (currentAttacks == possibleAttacks)
            ue.canEndEarlier = true;

        attacksSpace += Time.unscaledDeltaTime;
    }

    private void SetPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnDisable()
    {
        Camera.main.orthographicSize = oldCamSize;
        currentAttacks = 0;
    }
}
