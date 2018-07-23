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

    #region Private Memebers
    protected Action m_CloseCallback;
    #endregion  

    #region Abstract Methods
    public abstract void Open();
    public abstract void Close();
    public abstract void AddCommentToUI(Comment comment);
    #endregion

    #region Unity Monobehaviours
    public virtual void Awake()
    {
        objectPool = new ObjectPool<CommentPanel>(5, 5, CommentPanelPrefab);
    }

    public virtual void OnEnable()
    {
        CHScriptObject.commentHandler = this;
    }
    #endregion

    #region Public Methods
    //Needs to be called in order to populate the annotation panel
    public virtual void InitAnnotationPanel(AnnotationNode annotationNode, Action closeCallback)
    {
        ClearCommentUI();
        m_AnnotationNode = annotationNode;
        m_CloseCallback = closeCallback;
        m_CommentPanels.Capacity = annotationNode.ThreadCount;

        //create the Original comment that the thread pool is based on.
        AddCommentToUI(annotationNode.MainThread);

        //Iterate through the thread of comments and create the reply comments
        foreach (Comment comment in annotationNode.Replies)
        {
            AddCommentToUI(comment);
        }
    }

    public virtual void ChangeContent(CommentPanel panelToEdit)
    {
        //TODO: enable the comment logic here
        m_CommentBeingEdited = panelToEdit;
    }

    public virtual CommentPanel GetCommentPanelFromPool()
    {
        CommentPanel commentPanel = objectPool.GetObjectFromPool();
        m_CommentPanels.Add(commentPanel);
        return commentPanel;
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

    public void ReplyToComment()
    {
        
    }    
    #endregion

    #region Protected Methods
    /// <summary>
    /// Adds a comment to the base annotation node
    /// </summary>
    protected virtual void AddCommentToAnnotationNode(Comment comment)
    {
        m_AnnotationNode.AddComment(comment);
    }

    protected virtual void PublishComment()
    {
        
    }
    
    /// Deletes the entire comment thread. This will delete the annotation from memory
    protected virtual void DeleteThread()
    {
        m_AnnotationNode.DeleteAnnotation();
        foreach (CommentPanel commentPanel in m_CommentPanels)
        {
            commentPanel.gameObject.SetActive(false);
        }
    }

    protected virtual void ClearCommentUI()
    {
        foreach(CommentPanel panels in  m_CommentPanels)
        {
            panels.gameObject.SetActive(false);
        }

        m_CommentPanels.Clear();        
    }
    #endregion
}
