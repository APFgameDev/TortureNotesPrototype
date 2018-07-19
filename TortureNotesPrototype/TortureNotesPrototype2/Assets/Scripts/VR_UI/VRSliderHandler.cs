using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(VRSelectableHandler))]
public class VRSliderHandler : MonoBehaviour, IPointerClickHandler
{
    Slider slider;
    RectTransform rectTransform;


	// Use this for initialization
	void Awake () {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        slider.value = MathUtility.CalculateDragValue(rectTransform, transform, eventData.position, (UnityEngine.UI.Scrollbar.Direction)slider.direction, slider.minValue, slider.maxValue);
    }
}
