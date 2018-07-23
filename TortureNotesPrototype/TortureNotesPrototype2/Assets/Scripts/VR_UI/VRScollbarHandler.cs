using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Scrollbar))]
public class VRScollbarHandler : VRSelectableHandler
{
    Scrollbar scrollBar;
    RectTransform rectTransform;

    // Use this for initialization
    void Awake()
    {
        scrollBar = GetComponent<Scrollbar>();
        rectTransform = GetComponent<RectTransform>();
        InitSelectableHandler();
    }

    public override void OnClickHeld(VRInteractionData vrInteraction)
    {
        scrollBar.value = MathUtility.CalculateDragValue(rectTransform, transform, Camera.main.WorldToScreenPoint(vrInteraction.GetClosestLaserPoint(transform.position)), scrollBar.direction, 0, 1);
    }
}
