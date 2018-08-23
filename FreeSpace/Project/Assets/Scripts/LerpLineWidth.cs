using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class LerpLineWidth : MonoBehaviour {
    [SerializeField]
    AnimationCurve scaleCurve;
    [SerializeField]
    [Range(0.1f, 5)]
    float scaleSpeed = 0.1f;

    [SerializeField]
    Vector2 maxScale;
    [SerializeField]
    Vector2 minScale;

    [SerializeField]
    public UnityEvent callsWhenDoneMaxScale;
    [SerializeField]
    public UnityEvent callsWhenDoneMinScale;

    System.Action callBackWhenDone;
    LineRenderer lineRenderer;
    bool currentlyScaling = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
            {
                StartCoroutine(ScaleStartWidthCoroutine(minScale.x, maxScale.x));
                StartCoroutine(ScaleEndWidthCoroutine(minScale.y, maxScale.y));
            }
            else
            {
                StartCoroutine(ScaleStartWidthCoroutine(maxScale.x, minScale.x));
                StartCoroutine(ScaleEndWidthCoroutine(maxScale.y, minScale.y));
            }
        }
    }
    

    public void StartScaleRect(bool maximize, System.Action aCallBackWhenDone)
    {
        callBackWhenDone = aCallBackWhenDone;

        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            if (maximize)
            {
                StartCoroutine(ScaleStartWidthCoroutine(minScale.x, maxScale.x));
                StartCoroutine(ScaleEndWidthCoroutine(minScale.y, maxScale.y));
            }
            else
            {
                StartCoroutine(ScaleStartWidthCoroutine(maxScale.x, minScale.x));
                StartCoroutine(ScaleEndWidthCoroutine(maxScale.y, minScale.y));
            }
        }
    }

    public bool GetIsCurrentlyScaling()
    {
        return currentlyScaling;
    }

    public void SetToMinScale()
    {
        lineRenderer.startWidth = minScale.x;
        lineRenderer.endWidth = minScale.y;
    }

    public void SetToMaxScale()
    {
        lineRenderer.startWidth = maxScale.x;
        lineRenderer.endWidth = maxScale.y;
    }

    IEnumerator ScaleStartWidthCoroutine(float start, float end)
    {
        float time = Mathf.InverseLerp(start, end, lineRenderer.startWidth);

        currentlyScaling = true;
        while (time < 1)
        {
            lineRenderer.startWidth = Mathf.Lerp(start, end, scaleCurve.Evaluate(time));

            yield return new WaitForEndOfFrame();

            time += Time.deltaTime * scaleSpeed;
        }

        lineRenderer.startWidth = Mathf.Lerp(start, end, scaleCurve.Evaluate(1));
        currentlyScaling = false;

        if (end == minScale.x)
            callsWhenDoneMinScale.Invoke();
        else
            callsWhenDoneMaxScale.Invoke();

        if (callBackWhenDone != null)
            callBackWhenDone();
    }

    IEnumerator ScaleEndWidthCoroutine(float start, float end)
    {
        float time = Mathf.InverseLerp(start, end, lineRenderer.endWidth);

        currentlyScaling = true;
        while (time < 1)
        {
            lineRenderer.endWidth = Mathf.Lerp(start, end, scaleCurve.Evaluate(time));

            yield return new WaitForEndOfFrame();

            time += Time.deltaTime * scaleSpeed;
        }

        lineRenderer.endWidth = Mathf.Lerp(start, end, scaleCurve.Evaluate(1));
        currentlyScaling = false;

        if (end == minScale.x)
            callsWhenDoneMinScale.Invoke();
        else
            callsWhenDoneMaxScale.Invoke();

        if (callBackWhenDone != null)
            callBackWhenDone();
    }
}
