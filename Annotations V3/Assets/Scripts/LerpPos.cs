using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPos : MonoBehaviour {

    Transform transform;
    [SerializeField]
    AnimationCurve lerpCurve;
    [SerializeField]
    [Range(0.1f, 5)]
    float lerpSpeed = 0.1f;

    [SerializeField]
    Vector3 maxPos;
    [SerializeField]
    Vector3 minPos;

    System.Action callBackWhenDone;

    bool currentlyLerping = false;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
    }

    public void LerpToMax(System.Action aCallBackWhenDone = null)
    {
        StartLerp(true, aCallBackWhenDone);
    }

    public void LerpToMin(System.Action aCallBackWhenDone = null)
    {
        StartLerp(false, aCallBackWhenDone);
    }

    public void StartSLerp(bool maximize)
    {
        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            if (maximize)
                StartCoroutine(LerpCoroutine(minPos, maxPos));
            else
                StartCoroutine(LerpCoroutine(maxPos, minPos));
        }
    }

    public void StartLerp(bool maximize, System.Action aCallBackWhenDone)
    {
        callBackWhenDone = aCallBackWhenDone;

        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            if (maximize)
                StartCoroutine(LerpCoroutine(minPos, maxPos));
            else
                StartCoroutine(LerpCoroutine(maxPos, minPos));
        }
    }

    public bool GetIsCurrentlyLerping()
    {
        return currentlyLerping;
    }

    public void SetToMinPos()
    {
        transform.localPosition = minPos;
    }

    public void SetToMaxPos()
    {
        transform.localPosition = maxPos;
    }

    IEnumerator LerpCoroutine(Vector3 start, Vector3 end)
    {
        float time = start.InverseLerp(end, transform.localPosition);

        currentlyLerping = true;
        while (time < 1)
        {
            transform.localPosition = Vector3.Lerp(start, end, lerpCurve.Evaluate(time));

            yield return new WaitForEndOfFrame();

            time += Time.deltaTime * lerpSpeed;
        }

        transform.localPosition = Vector3.Lerp(start, end, lerpCurve.Evaluate(1));
        currentlyLerping = false;


        if (callBackWhenDone != null)
            callBackWhenDone();
    }
}
