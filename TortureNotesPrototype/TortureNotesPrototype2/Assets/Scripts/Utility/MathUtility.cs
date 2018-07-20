using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    static public float CalculateDragValue(RectTransform rectTransform, Transform dragTransform, Vector2 pointerScreenPos, UnityEngine.UI.Scrollbar.Direction dir, float minValueRange, float maxValueRange)
    {
        Vector2 localRectPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pointerScreenPos, Camera.main, out localRectPoint);

        float maxValue = 0;
        float localValue = 0;
        float minValue = 0;

        switch (dir)
        {
            case UnityEngine.UI.Scrollbar.Direction.LeftToRight:
                maxValue = rectTransform.rect.width;
                localValue = localRectPoint.x + rectTransform.offsetMax.x;
                break;
            case UnityEngine.UI.Scrollbar.Direction.RightToLeft:
                minValue = rectTransform.rect.width;
                localValue = localRectPoint.x + rectTransform.offsetMax.x;
                break;
            case UnityEngine.UI.Scrollbar.Direction.BottomToTop:
                maxValue = rectTransform.rect.height;
                localValue = localRectPoint.y + rectTransform.offsetMax.y;
                break;
            case UnityEngine.UI.Scrollbar.Direction.TopToBottom:
                minValue = rectTransform.rect.height;
                localValue = localRectPoint.y + rectTransform.offsetMax.y;
                break;
        }

        float sliderPercent = Mathf.InverseLerp(minValue, maxValue, localValue);

        return Mathf.Lerp(minValueRange, maxValueRange, sliderPercent);
    }

    static public void AddTriggerBoxToRectTransform(RectTransform rectTransform)
    {
        Rect rect = rectTransform.rect;
        BoxCollider boxCollider = rectTransform.gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(rect.width, rect.height, 0.1f);
    }

}
