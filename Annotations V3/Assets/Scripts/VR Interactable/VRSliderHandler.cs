using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VRSliderHandler : VRSelectableHandler
{
    Slider slider;

	// Use this for initialization
	void Awake ()
    {
        slider = GetComponent<Slider>();
        InitSelectableHandler();
    }

    public override void OnClickHeld(VRInteractionData vrInteraction)
    {
        slider.value = RectTransformMathUtility.CalculateDragValue(selectableRect, slider.handleRect, vrInteraction.GetClosestLaserPointOnPlane(transform.position, transform.forward), (Scrollbar.Direction)slider.direction, 0, 1);
    }
}
