using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScaleRect : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField]


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void StartScaleRect(Vector3 newScale)
    {
        StopAllCoroutines();
       // StartCoroutine(ScaleRect(newScale));
    }



    //IEnumerator ScaleRect(Vector3 newScale)
    //{
        

    //}
}
