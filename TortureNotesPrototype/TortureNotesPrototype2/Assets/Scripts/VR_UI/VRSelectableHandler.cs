﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(RectTransform))]
public class VRSelectableHandler : VRInteractable
{
    int numSelecting = 0;
    Selectable selectable;
    IPointerClickHandler clickHandler;

    [SerializeField]
    protected RectTransform selectableRect;

    private void Awake()
    {
        InitSelectableHandler();
    }
    public void InitSelectableHandler()
    {
        selectable = GetComponent<Selectable>();
        clickHandler = GetComponent<IPointerClickHandler>();

        if (selectableRect == null)
            selectableRect = GetComponent<RectTransform>();
        if (GetComponent<BoxCollider>() == null)
            MathUtility.AddTriggerBoxToRectTransform(selectableRect, Vector3.forward * -0.1f, gameObject);
    }

    override public void OnHoverEnter(VRInteractionData vrInteraction)
    {
        if (numSelecting == 0)
        {
            selectable.OnPointerEnter(new PointerEventData(EventSystem.current));
        }

        numSelecting++;

        base.OnHoverEnter(vrInteraction);
    }

    override public void OnHoverExit(VRInteractionData vrInteraction)
    {
        numSelecting--;

        if (numSelecting == 0)
        {
            selectable.OnPointerExit(new PointerEventData(EventSystem.current));
        }

        base.OnHoverExit(vrInteraction);
    }

    override public void OnClick(VRInteractionData vrInteraction)
    {
        base.OnClick(vrInteraction);

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Camera.main.WorldToScreenPoint(vrInteraction.GetClosestLaserPoint(transform.position));

        if (clickHandler != null)
            clickHandler.OnPointerClick(pointerEventData);

        base.OnClick(vrInteraction);
    }
}