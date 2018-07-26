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
    ScaleRect scaleMainCommentRect;

    [SerializeField]
    Text threadTopicText;

    [SerializeField]
    Text authorText;

    [SerializeField]
    Text dateText;

    [SerializeField]
    VRGrabbable m_nodeHighlightArea;

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

        scaleThreadTopicRect.SetToMinScale();

        grabEnabled = false;
        ToggleEditableText(false);
        m_nodeHighlightArea.grabEnabled = false;

        gameObject.SetActive(true);      
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        if (m_nodeHighlightArea)
            lineRenderer.SetPositions(new Vector3[] { startOfLine.position, m_nodeHighlightArea.transform.position });
    }

    public void ScaleThread(bool expanding,System.Action onComplete = null)
    {
        if(expanding)
        {
            scaleThreadTopicRect.StartScaleRect(true, onComplete += ScaleToMaxMainComment);
        }
        else
        {
            scaleMainCommentRect.StartScaleRect(false, ScaleToMinThreadTopic);
        }


    }

    void ScaleToMaxMainComment()
    {
        scaleMainCommentRect.StartScaleRect(true);
    }

    void ScaleToMinThreadTopic()
    {
        scaleThreadTopicRect.StartScaleRect(false);
    }

    public override void OnHoverEnter(VRInteractionData vrInteraction)
    {
        if (tagHandler.tagHandlerMode != TagHandlerMode.Regular)
            return;

        ScaleThread(true);
    }

    public override void OnHoverExit(VRInteractionData vrInteraction)
    {
        if (tagHandler.tagHandlerMode != TagHandlerMode.Regular)
            return;

        ScaleThread(false);
    }

    public void ViewCommentsInAnnotation()
    {
        if (tagHandler.tagHandlerMode == TagHandlerMode.Delete)
        {
            tagHandler.DeleteThread(this);
        }
        else
        {
            tagHandler.ViewAnnotationComments(this, target);
        }
    }

    public void ToggleEditableText(bool isEditing)
    {
        threadTopicText.GetComponent<VREditableText>().enabled = isEditing;

        if (isEditing == false)
        {
            Comment comment = annotationNode.MainThread;

            comment.content = threadTopicText.text;

            annotationNode.MainThread = comment;
        }
    }

    public void ToggleEditPlacement(bool canBePlaced)
    {
        m_nodeHighlightArea.grabEnabled = canBePlaced;
        grabEnabled = canBePlaced;
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

    public void SaveData()
    {
        annotationNode.AnnotationStartPos = target.InverseTransformPoint(m_nodeHighlightArea.transform.position);
        annotationNode.AnnotationScale = m_nodeHighlightArea.transform.lossyScale.x;
        annotationNode.AnnotationEndPos = target.InverseTransformPoint(transform.position);
    }
}
