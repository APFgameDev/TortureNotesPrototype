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

    bool annotationsVisible = false;
    bool viewingAnnotationComments = false;

    private void Awake()
    {
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
        if (viewingAnnotationComments == false)
        {
            annotationsVisible = !annotationsVisible;

            if (annotationsVisible)
            {
                for (int i = 0; i < tagData.annotationNodes.Count; i++)
                {
                    NodeComponent nodeComponent = annotationNodePool.GetObjectFromPool();
                    nodeComponent.InitNodeComponent(tagData.annotationNodes[i], target, this);
                    nodeComponent.gameObject.SetActive(true);
                }
            }
            else
            {
                annotationNodePool.ReturnAllActiveToPool();
            }
        }
    }

    public void ViewAnnotation(NodeComponent aNodeComponent,Transform target)
    {
        viewingAnnotationComments = !viewingAnnotationComments;
        if (viewingAnnotationComments)
        {
            annotationsVisible = false;

            annotationNodePool.ReturnAllActiveToPool(aNodeComponent);

            commentHandlerSO.commentHandler.transform.parent = transform;
            commentHandlerSO.commentHandler.transform.localPosition = Vector3.zero + Vector3.down * 2f;
            commentHandlerSO.commentHandler.transform.localRotation = Quaternion.identity;

            commentHandlerSO.commentHandler.GetComponent<ScaleRect>().ScaleRectToMax();
            commentHandlerSO.commentHandler.InitAnnotationPanel(aNodeComponent.annotationNode);
            commentHandlerSO.commentHandler.Open();
        }
        else
        {
            commentHandlerSO.commentHandler.GetComponent<ScaleRect>().ScaleRectToMin();
            commentHandlerSO.commentHandler.Close();
            aNodeComponent.gameObject.SetActive(false);

            ToggleAnnotationView();
        }
    }
}
