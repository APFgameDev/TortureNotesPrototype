using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Scrollbar))]
[RequireComponent(typeof(VRSelectableHandler))]
public class VRScollbarHandler : MonoBehaviour, IPointerClickHandler
{

    Scrollbar scrollBar;
    RectTransform rectTransform;


    // Use this for initialization
    void Awake()
    {
        scrollBar = GetComponent<Scrollbar>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        scrollBar.value = MathUtility.CalculateDragValue(rectTransform, transform, eventData.position, scrollBar.direction, 0, 1);
    }
}
