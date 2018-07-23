using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Annotation.NS_Data;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class NodeComponent : VRGrabbable
{
    public AnnotationNode annotationNode;

    [SerializeField]
    ScaleRect scaleThreadTopicRect;

    [SerializeField]
    Text threadTopicText;

    [SerializeField]
    Text authorText;

    [SerializeField]
    Text dateText;

    [SerializeField]
    VRInteractable m_nodeHighlightArea;

    [SerializeField]
    Transform startOfLine;

    [SerializeField]
    Text numCommentsText;

    Transform target;
    LineRenderer lineRenderer;
    TagHandler tagHandler;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        m_nodeHighlightArea.gameObject.SetActive(false);
    }

    public void InitNodeComponent(AnnotationNode aAnnotationNode, Transform aTarget, TagHandler aTagHandler)
    {
        target = aTarget;

        annotationNode = aAnnotationNode;

        transform.position = target.position + annotationNode.AnnotationEndPos;

        m_nodeHighlightArea.transform.parent = aTarget;

        m_nodeHighlightArea.transform.position = target.position + annotationNode.AnnotationStartPos;

        m_nodeHighlightArea.transform.localScale = Vector3.one * annotationNode.AnnotationScale;

        numCommentsText.text = annotationNode.ThreadCount.ToString();
        threadTopicText.text = aAnnotationNode.MainThread.content;
        authorText.text = aAnnotationNode.MainThread.author;
        dateText.text = aAnnotationNode.MainThread.date;

        tagHandler = aTagHandler;
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
        tagHandler.ViewAnnotation(this, target);
    }

    private void OnDisable()
    {
        if (m_nodeHighlightArea != null)
            m_nodeHighlightArea.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_nodeHighlightArea.gameObject.SetActive(true);
    }
}
