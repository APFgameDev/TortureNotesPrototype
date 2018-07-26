using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnotationPanel : MonoBehaviour
{
    [SerializeField]
    Text title;

    [SerializeField]
    Text mainComment;

    public void InitAnnotationPanel(NS_Annotation.NS_Data.AnnotationNode annotationNode)
    {
        title.text = annotationNode.m_AnnotationTitle;
        mainComment.text = annotationNode.MainThread.content;
    }

}
