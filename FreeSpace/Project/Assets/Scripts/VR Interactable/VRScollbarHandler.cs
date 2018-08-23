using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Scrollbar))]
public class VRScollbarHandler : VRSelectableHandler
{
    Scrollbar scrollBar;

    // Use this for initialization
    void Awake()
    {
        scrollBar = GetComponent<Scrollbar>();
        InitSelectableHandler();
    }

    public override void OnClickHeld(VRInteractionData vrInteraction)
    {
       scrollBar.value =  RectTransformMathUtility.CalculateDragValue(selectableRect, scrollBar.handleRect, vrInteraction.GetClosestLaserPointOnPlane(vrInteraction.m_hitPoint, transform.forward), scrollBar.direction, 0, 1);
    }
}
