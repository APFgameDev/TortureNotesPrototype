using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(RectTransform))]
public class VRSelectableHandler : MonoBehaviour
{
    int numSelecting = 0;
    Selectable selectable;
    IPointerClickHandler clickHandler;


    private void Awake()
    {
        selectable = GetComponent<Selectable>();
        clickHandler = GetComponent<IPointerClickHandler>();
        Rect rect = GetComponent<RectTransform>().rect;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(rect.width, rect.height, 0.1f);
    }

    public void IncrementSelection()
    {
        if (numSelecting == 0)
        {
            selectable.OnPointerEnter(new PointerEventData(EventSystem.current));
        }

        numSelecting++;
    }

    public void DecrementSelection()
    {
        numSelecting--;

        if (numSelecting == 0)
        {
            selectable.OnPointerExit(new PointerEventData(EventSystem.current));
        }
    }

    public void Select(Vector3 hitPoint)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Camera.main.WorldToScreenPoint(hitPoint);

        if (clickHandler != null)
            clickHandler.OnPointerClick(pointerEventData);
    }
}
