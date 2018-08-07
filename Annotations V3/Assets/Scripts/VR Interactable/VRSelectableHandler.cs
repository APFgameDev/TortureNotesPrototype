using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(RectTransform))]
public class VRSelectableHandler : VR_UI_Interactable
{
    int numSelecting = 0;
    Selectable selectable;
    IPointerClickHandler clickHandler;

    private void Awake()
    {
        InitSelectableHandler();
        InitVR_UI_Interactable();
    }
    public void InitSelectableHandler()
    {
        selectable = GetComponent<Selectable>();
        clickHandler = GetComponent<IPointerClickHandler>();

        InitVR_UI_Interactable();
    }

    override public void OnHoverEnter(VRInteractionData vrInteraction)
    {
        if (numSelecting == 0 && selectable != null)
        {
            selectable.OnPointerEnter(new PointerEventData(EventSystem.current));
        }

        numSelecting++;

        base.OnHoverEnter(vrInteraction);
    }

    override public void OnHoverExit(VRInteractionData vrInteraction)
    {
        numSelecting--;

        if (numSelecting == 0 && selectable != null)
        {
            selectable.OnPointerExit(new PointerEventData(EventSystem.current));
        }

        base.OnHoverExit(vrInteraction);
    }

    override public void OnClick(VRInteractionData vrInteraction)
    {
        base.OnClick(vrInteraction);

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Camera.main.WorldToScreenPoint(m_hitPoint);

        if (clickHandler != null)
            clickHandler.OnPointerClick(pointerEventData);

        base.OnClick(vrInteraction);
    }
}