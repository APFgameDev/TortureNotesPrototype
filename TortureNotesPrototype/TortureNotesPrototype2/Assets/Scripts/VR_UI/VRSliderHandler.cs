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
        Vector2 screenPosLeft = Camera.main.WorldToScreenPoint(transform.position - transform.right * rectTransform.rect.min.x);
        Vector2 localRectPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, Camera.main, out localRectPoint);

        //TODO check slider direction and adjust calculation accordingly
        float sliderPercent = Mathf.InverseLerp(0, rectTransform.rect.width, localRectPoint.x + rectTransform.offsetMax.x);

        slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, sliderPercent);
    }
}
