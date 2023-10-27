using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverInfo : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject showing;

    public void OnSelect(BaseEventData eventData)
    {
        showing.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        showing.SetActive(false);
    }
}
