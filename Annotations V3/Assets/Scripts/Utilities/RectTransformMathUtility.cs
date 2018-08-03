using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformMathUtility
{
    static public float CalculateDragValue(RectTransform rectTransform, RectTransform handleRect, Vector3 wordPos, UnityEngine.UI.Scrollbar.Direction dir, float minValueRange, float maxValueRange)
    {
        Vector3 localRectPoint;
        localRectPoint = rectTransform.InverseTransformPoint(wordPos);

        float maxValue = 0;
        float localValue = 0;
        float minValue = 0;

        switch (dir)
        {
            case UnityEngine.UI.Scrollbar.Direction.LeftToRight:

                maxValue = rectTransform.rect.xMax + handleRect.rect.xMin;
                minValue = rectTransform.rect.xMin + handleRect.rect.xMax;
                localValue = localRectPoint.x;
                break;

            case UnityEngine.UI.Scrollbar.Direction.RightToLeft:
         
                maxValue = rectTransform.rect.xMin + handleRect.rect.xMax;
                minValue = rectTransform.rect.xMax + handleRect.rect.xMin;
                localValue = localRectPoint.x;
                break;

            case UnityEngine.UI.Scrollbar.Direction.BottomToTop:

                maxValue = rectTransform.rect.yMax + handleRect.rect.yMin;
                minValue = rectTransform.rect.yMin + handleRect.rect.yMax;
                localValue = localRectPoint.y;
                break;

            case UnityEngine.UI.Scrollbar.Direction.TopToBottom:

                maxValue = rectTransform.rect.yMin + handleRect.rect.yMax;
                minValue = rectTransform.rect.yMax  + handleRect.rect.yMin;    
                localValue = localRectPoint.y;
                break;
        }

        float sliderPercent = Mathf.InverseLerp(minValue, maxValue, localValue );

        return Mathf.Lerp(minValueRange, maxValueRange, sliderPercent);
    }

    static public BoxCollider AddTriggerBoxToRectTransform(RectTransform rectTransfrom, Vector3 offset, float zThickness)
    {
        if (rectTransfrom != null)
        {
            BoxCollider boxCollider = rectTransfrom.gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            SetBoxCollidersizeToRect(rectTransfrom.rect, offset, boxCollider, zThickness);
            return boxCollider;
        }
        else
            return null;
    }

    static public void SetBoxCollidersizeToRect(Rect rect, Vector3 offset, BoxCollider boxCollider,float zThickness)
    {
        if (boxCollider != null && rect != null && offset != null)
        {
            boxCollider.size = new Vector3(rect.xMax - rect.xMin, rect.yMax - rect.yMin, zThickness);
            boxCollider.center = offset + new Vector3((rect.xMax + rect.xMin) * 0.5f, (rect.yMax + rect.yMin) * 0.5f, zThickness * 0.5f);
        }
    }
}
