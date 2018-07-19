using System.Collections;
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

    private void Awake()
    {
        InitSelectableHandler();
    }
    public void InitSelectableHandler()
    {
        selectable = GetComponent<Selectable>();
        clickHandler = GetComponent<IPointerClickHandler>();
        Rect rect = GetComponent<RectTransform>().rect;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(rect.width, rect.height, 0.1f);
    }

    override public void OnHoverEnter(VRInteractionData vrInteraction)
    {
        if (numSelecting == 0)
        {
            selectable.OnPointerEnter(new PointerEventData(EventSystem.current));
        }

        numSelecting++;
    }

    override public void OnHoverExit(VRInteractionData vrInteraction)
    {
        numSelecting--;

        if (numSelecting == 0)
        {
            selectable.OnPointerExit(new PointerEventData(EventSystem.current));
        }
    }

    override public void OnClick(VRInteractionData vrInteraction)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Camera.main.WorldToScreenPoint(vrInteraction.pos);

        if (clickHandler != null)
            clickHandler.OnPointerClick(pointerEventData);
    }
}


public class VRInteractable : MonoBehaviour
{
    public virtual void OnHoverEnter(VRInteractionData vrInteraction)
    {

    }
    public virtual void OnHoverExit(VRInteractionData vrInteraction)
    {

    }
    public virtual void OnClick(VRInteractionData vrInteraction)
    {

    }
    public virtual void OnClickHeld(VRInteractionData vrInteraction)
    {

    }
    public virtual void OnClickRelease(VRInteractionData vrInteraction)
    {

    }
}

public struct VRInteractionData
{
    public Vector3 pos;
    public Transform handTrans;
}