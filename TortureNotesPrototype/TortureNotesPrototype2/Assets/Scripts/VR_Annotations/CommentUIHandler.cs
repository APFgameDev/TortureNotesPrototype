using NS_Annotation.NS_Data;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommentUIHandler : MonoBehaviour
{
    #region Public Members
    public CommentHandlerSO CHScriptObject;
    public GameObject CommentPanelPrefab;
    public UnityEngine.Events.UnityEvent OnThreadDelete;
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
    public virtual void InitAnnotationPanel(AnnotationNode annotationNode)
    {
        ClearCommentUI();
        m_AnnotationNode = annotationNode;
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
    public virtual void DeleteComment(CommentPanel comment)
    {
        if (comment.Comment == m_AnnotationNode.MainThread)
        {
            DeleteThread();
        }
        else
        {
            DeleteReply(comment);
        }
    }

    public void ReplyToComment(Comment comment)
    {
        AddCommentToUI(comment);
        m_AnnotationNode.AddComment(comment);
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
    
    /// Deletes the entire comment thread. This will delete the annotation from memory
    protected virtual void DeleteThread()
    {
        m_AnnotationNode.DeleteAnnotation();
        foreach (CommentPanel commentPanel in m_CommentPanels)
        {
            commentPanel.gameObject.SetActive(false);
        }

        OnThreadDelete.Invoke();        
    }

    protected  virtual void DeleteReply(CommentPanel comment)
    {
        m_AnnotationNode.RemoveComment(comment.Comment);
        comment.gameObject.SetActive(false);
        m_CommentPanels.Remove(comment);
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
