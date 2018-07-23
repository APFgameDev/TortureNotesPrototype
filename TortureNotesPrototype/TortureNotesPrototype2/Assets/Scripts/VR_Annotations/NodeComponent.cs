using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Annotation.NS_Data;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class NodeComponent : VRInteractable
{
    AnnotationNode annotationNode;

    [SerializeField]
    ScaleRect scaleThreadTopicRect;

    [SerializeField]
    VRInteractable m_nodeHighlightArea;

    [SerializeField]
    Transform startOfLine;

    [SerializeField]
    Text numCommentsText;

    Transform target;
    LineRenderer lineRenderer;
    CommentUIHandler commentUIHandler;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void InitNodeComponent(AnnotationNode aAnnotationNode, Transform aTarget, CommentUIHandler commentHandler)
    {
        target = aTarget;

        annotationNode = aAnnotationNode;

        transform.position = target.position + annotationNode.AnnotationEndPos;
        m_nodeHighlightArea.transform.position = target.position + annotationNode.AnnotationStartPos;

        m_nodeHighlightArea.transform.localScale = Vector3.one * annotationNode.AnnotationScale;

        numCommentsText.text = annotationNode.ThreadCount.ToString();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        if (m_nodeHighlightArea)
            lineRenderer.SetPositions(new Vector3[] { startOfLine.position, m_nodeHighlightArea.transform.position });
    }


    public override void OnHoverEnter(VRInteractionData vrInteraction)
    {
        scaleThreadTopicRect.ScaleRectToMax();
    }

    public override void OnHoverExit(VRInteractionData vrInteraction)
    {
        scaleThreadTopicRect.ScaleRectToMin();
    }

    public void ViewCommentsInAnnotation()
    {
        commentUIHandler.InitAnnotationPanel(annotationNode, OnReOpenAnnotationView);
        commentUIHandler.Open();
    }

    void OnReOpenAnnotationView()
    {

    }



}
