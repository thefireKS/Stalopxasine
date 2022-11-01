using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisemanSoulGirl : MonoBehaviour
{
    public int possibleAttacks = 3;
    [SerializeField] private GameObject swingAttack;
    
    private UltimateEnergy ue;
    
    private int currentAttacks = 0;
    private float attacksSpace = 0f;

    [SerializeField] private Transform[] positions;
    
    private float oldCamSize;
    private void Start()
    {
        ue = GetComponentInParent<UltimateEnergy>();
    }
    
    private void OnEnable()
    {
        oldCamSize = Camera.main.orthographicSize;
        Camera.main.orthographicSize *= 1.25f;
    }
    
    private void Update()
    {
        SetPosition();
        
        if (Input.GetMouseButton(0) && currentAttacks < possibleAttacks && attacksSpace > 0.5f)
        {
            attacksSpace = 0f;
            for (int t = 0; t < positions.Length; t++)
            {
                Debug.Log(positions[t].gameObject.name);
                Instantiate(swingAttack, positions[t].position, Quaternion.Euler(0,0,positions[t].rotation.eulerAngles.z));
            }
            currentAttacks++;
        }

        if (currentAttacks == possibleAttacks)
            ue.canEndEarlier = true;

        attacksSpace += Time.deltaTime;
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
