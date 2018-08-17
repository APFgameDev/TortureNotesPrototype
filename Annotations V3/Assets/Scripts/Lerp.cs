using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class LerpEvent : UnityEvent<float>
{
}

public class Lerp : MonoBehaviour {

    [SerializeField]
    AnimationCurve scaleCurve;
    [SerializeField]
    [Range(0.1f, 5)]
    float scaleSpeed = 0.1f;

    [SerializeField]
    float maxScale;
    [SerializeField]
    float minScale;

    [SerializeField]
    public UnityEvent callsWhenDoneMaxScale;
    [SerializeField]
    public UnityEvent callsWhenDoneMinScale;

    [SerializeField]
    float lerpValue = 0;

    System.Action callBackWhenDone;

    [SerializeField]
    LerpEvent lerpCallBack;
    bool currentlyScaling = false;

    public void LerpToMax(System.Action aCallBackWhenDone = null)
    {
        StartLerp(true, aCallBackWhenDone);
    }

    public void LerpToMin(System.Action aCallBackWhenDone = null)
    {
        StartLerp(false, aCallBackWhenDone);
    }

    public void StartLerp(bool maximize)
    {
        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            if (maximize)
                StartCoroutine(ScaleStartCoroutine(minScale, maxScale));
            else
                StartCoroutine(ScaleStartCoroutine(maxScale, minScale));
        }
    }


    public void StartLerp(bool maximize, System.Action aCallBackWhenDone)
    {
        callBackWhenDone = aCallBackWhenDone;

        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            if (maximize)
                StartCoroutine(ScaleStartCoroutine(minScale, maxScale));
            else
                StartCoroutine(ScaleStartCoroutine(maxScale, minScale));
        }
    }

    public bool GetIsCurrentlyLerping()
    {
        return currentlyScaling;
    }

    public void SetToMin()
    {
        lerpCallBack.Invoke(lerpValue = minScale);
    }

    public void SetToMax()
    {
        lerpCallBack.Invoke(lerpValue = maxScale);
    }

    IEnumerator ScaleStartCoroutine(float start, float end)
    {
        float time = Mathf.InverseLerp(start, end, lerpValue);

        currentlyScaling = true;
        while (time < 1)
        {
            lerpValue = Mathf.Lerp(start, end, scaleCurve.Evaluate(time));

            lerpCallBack.Invoke(lerpValue);
            yield return new WaitForEndOfFrame();

            time += Time.deltaTime * scaleSpeed;
        }

        lerpValue = Mathf.Lerp(start, end, scaleCurve.Evaluate(1));
        currentlyScaling = false;

        if (end == minScale)
            callsWhenDoneMinScale.Invoke();
        else
            callsWhenDoneMaxScale.Invoke();

        if (callBackWhenDone != null)
            callBackWhenDone();
    }
}
