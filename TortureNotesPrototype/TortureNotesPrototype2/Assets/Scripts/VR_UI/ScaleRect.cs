using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScaleRect : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField]
    AnimationCurve scaleCurve;
    [SerializeField]
    [Range(0.1f,5)]
    float scaleSpeed;

    [SerializeField]
    Vector3 maxScale;
    [SerializeField]
    Vector3 minScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ScaleRectToMax()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleRectCoroutine(minScale, maxScale));
    }

    public void ScaleRectToMin()
    {
        StopAllCoroutines();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ScaleRectCoroutine(maxScale, minScale));
        }
    }

    IEnumerator ScaleRectCoroutine(Vector3 start, Vector3 end)
    {
        float time = start.InverseLerp(end, rectTransform.localScale);
        Vector3 startScale = rectTransform.localScale;

        while (time < 1)
        {
            rectTransform.localScale = Vector3.Lerp(start, end, scaleCurve.Evaluate(time));

            yield return new WaitForEndOfFrame();

            time += Time.deltaTime * scaleSpeed;
        }

        rectTransform.localScale = Vector3.Lerp(start, end, scaleCurve.Evaluate(1));
    }
}
