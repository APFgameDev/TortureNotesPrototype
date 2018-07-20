using NS_Annotation.NS_Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagHandler : VRGrabbable
{
   
    [SerializeField]
    Transform startOfLine;
    [SerializeField]
    LineRenderer line;

    [SerializeField]
    Text titleText;
    [SerializeField]
    Text numberOfAnnotationsText;
    [SerializeField]
    Text descriptionText;

    [SerializeField]
    CommentHandlerSO commentHandlerSO;
    [SerializeField]
    Transform target;
    [SerializeField]
    RectTransform descriptionBox;


    private void Awake()
    {
    }

    public void PlaceTag(Transform aTarget,Tag tagData)
    {
        transform.position = aTarget.position + tagData.localPos;
        descriptionText.text = tagData.description;

        target = aTarget;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        if (target)
            line.SetPositions(new Vector3[] { startOfLine.position, target.position });
    }

}
