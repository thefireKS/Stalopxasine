using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject showing;
    void Start()
    {
        showing.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        showing.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        showing.SetActive(false);
    }
}
