using NS_Annotation.NS_Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class TagHandler : VRGrabbable
{ 
    [SerializeField]
    Transform startOfLine;

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
    ScaleRect descriptionScaleRect;

    LineRenderer lineRenderer;
    Tag tagData;

    ObjectPool<NodeComponent> annotationNodePool;

    List<NodeComponent> m_activeAnnotationNodes;

    [SerializeField]
    GameObject AnnotationNodePrefab;

    bool onAnnotationView = false;

    private void Awake()
    {
        PlaceTag(target, new Tag());
        annotationNodePool = new ObjectPool<NodeComponent>(5, 5, AnnotationNodePrefab);
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void PlaceTag(Transform aTarget,Tag aTagData)
    {
        tagData = aTagData;

        transform.position = aTarget.position + aTagData.localPos;
        descriptionText.text = aTagData.description;
        numberOfAnnotationsText.text = aTagData.AnnotationCount.ToString();

        target = aTarget;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        if (target)
            lineRenderer.SetPositions(new Vector3[] { startOfLine.position, target.position });
    }

    public override void OnHoverEnter(VRInteractionData vrInteraction)
    {
        descriptionScaleRect.ScaleRectToMax();
    }

    public override void OnHoverExit(VRInteractionData vrInteraction)
    {
        descriptionScaleRect.ScaleRectToMin();
    }

    public void ToggleAnnotationView()
    {
        onAnnotationView = !onAnnotationView;

        if (onAnnotationView)
        {
            for (int i = 0; i < tagData.annotationNodes.Count; i++)
            {
                NodeComponent nodeComponent = annotationNodePool.GetObjectFromPool();
                nodeComponent.InitNodeComponent(tagData.annotationNodes[i], target, commentHandlerSO.commentHandler);
            }
        }
        else
        {
            annotationNodePool.ReturnAllActiveToPool();
        }

    }
}
