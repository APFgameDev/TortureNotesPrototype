using NS_Annotation.NS_Data;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class CommentUIHandler : MonoBehaviour
{
    #region Public Members
    public CommentHandlerSO CHScriptObject;
    public GameObject CommentPanelPrefab;
    //TODO: add a reference to the keyboard in here. This will be enabled and disabled when ever the user goes to edit or create a comment
    #endregion

    #region protected Members
    protected List<CommentPanel> m_CommentPanels = new List<CommentPanel>();

    protected AnnotationNode m_AnnotationNode;

    //Object pool of all comment panels that are going to be used in the CommentUI
    protected ObjectPool<CommentPanel> objectPool;

    protected CommentPanel m_CommentBeingEdited;
    #endregion

    #region Abstract Methods
    public abstract void Open();
    public abstract void Close();
    public abstract void AddCommentToUI();
    #endregion  

    public virtual void Awake()
    {
        objectPool = new ObjectPool<CommentPanel>(5, 5, CommentPanelPrefab);
    }

    public virtual void OnEnable()
    {
        CHScriptObject.commentHandler = this;
    }

    public virtual void InitAnnotationPanel(AnnotationNode annotationNode)
    {
        m_CommentPanels.Capacity = annotationNode.ThreadCount;        
        foreach (Comment comment in annotationNode.Replies)
        {
            CreateAnnotationPanel(comment);
        }
    }

    /// <summary>
    /// Adds a comment to the base annotation node
    /// </summary>
    protected virtual void AddCommentToAnnotationNode(Comment comment)
    {
        m_AnnotationNode.AddComment(comment);
    }

    protected virtual void CreateAnnotationPanel(Comment commentData)
    {
        CommentPanel commentPanel = GetCommentPanelFromPool();
        commentPanel.InitCommentPanel(commentData, this);
        m_CommentPanels.Add(commentPanel);
    }

    public virtual void ChangeContent(CommentPanel panelToEdit)
    {
        //TODO: enable the comment logic here
        m_CommentBeingEdited = panelToEdit;
    }

    protected virtual void PublishComment()
    {
        m_CommentBeingEdited.authorText.text = "Jim Bob, Joe";

        DateTime date = new DateTime();
        m_CommentBeingEdited.dateText.text = date.Day.ToString() + " / " + date.Month.ToString() + " / " + date.Year.ToString();
    }

    public virtual CommentPanel GetCommentPanelFromPool()
    {
        return objectPool.GetObjectFromPool();       
    }

    /// <summary>
    /// Deletes a specific comment out of the thread.
    /// </summary>
    public virtual void DeleteComment(Comment comment)
    {
        if (comment == m_AnnotationNode.MainThread)
        {
            DeleteThread();
        }
        else 
        {
            m_AnnotationNode.RemoveComment(comment);
        }
    }

    /// Deletes the entire comment thread. This will delete the annotation from memory
    protected virtual void DeleteThread()
    {
        m_AnnotationNode.DeleteAnnotation();
        foreach(CommentPanel commentPanel in m_CommentPanels)
        {
            commentPanel.gameObject.SetActive(false);
        }
    }    
}
