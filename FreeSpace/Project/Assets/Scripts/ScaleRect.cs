﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleRect : MonoBehaviour
{
    Transform rectTransform;
    [SerializeField]
    AnimationCurve scaleCurve;
    [SerializeField]
    [Range(0.1f,5)]
    float scaleSpeed = 0.1f;

    [SerializeField]
    Vector3 maxScale;
    [SerializeField]
    Vector3 minScale;

    [SerializeField]
    public UnityEvent callsWhenDoneMaxScale;
    [SerializeField]
    public UnityEvent callsWhenDoneMinScale;

    System.Action callBackWhenDone;

    bool currentlyScaling = false;

    private void Awake()
    {
        rectTransform = GetComponent<Transform>();
    }

    public void ScaleRectToMax(System.Action aCallBackWhenDone = null)
    {
        StartScaleRect(true, aCallBackWhenDone);
    }

    public void ScaleRectToMin(System.Action aCallBackWhenDone = null)
    {
        StartScaleRect(false, aCallBackWhenDone);
    }

    public void StartScaleRect(bool maximize)
    {
        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            if (maximize)
                StartCoroutine(ScaleRectCoroutine(minScale, maxScale));
            else
                StartCoroutine(ScaleRectCoroutine(maxScale, minScale));
        }
    }

    public void StartScaleRect(bool maximize, System.Action aCallBackWhenDone)
    {
        callBackWhenDone = aCallBackWhenDone;

        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            if (maximize)
                StartCoroutine(ScaleRectCoroutine(minScale, maxScale));
            else
                StartCoroutine(ScaleRectCoroutine(maxScale, minScale));
        }
    }

    public bool GetIsCurrentlyScaling()
    {
        return currentlyScaling;
    }

    public void SetToMinScale()
    {
        rectTransform.localScale = minScale;
    }

    public void SetToMaxScale()
    {
        rectTransform.localScale = maxScale;
    }

    IEnumerator ScaleRectCoroutine(Vector3 start, Vector3 end)
    {
        float time = start.InverseLerp(end, rectTransform.localScale);

        currentlyScaling = true;
        while (time < 1)
        {
            rectTransform.localScale = Vector3.Lerp(start, end, scaleCurve.Evaluate(time));

            yield return new WaitForEndOfFrame();

            time += Time.deltaTime * scaleSpeed;
        }

        rectTransform.localScale = Vector3.Lerp(start, end, scaleCurve.Evaluate(1));
        currentlyScaling = false;

        yield return new WaitForEndOfFrame();

        if (end == minScale)
            callsWhenDoneMinScale.Invoke();
        else
            callsWhenDoneMaxScale.Invoke();

        if (callBackWhenDone != null)
            callBackWhenDone();
    }
}
