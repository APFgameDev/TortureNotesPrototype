using NS_Annotation.NS_Data;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommentUIHandler : MonoBehaviour
{
    #region Public Members
    public List<CommentPanel> m_CommentPanels = new List<CommentPanel>();
    #endregion

    #region Private Members
    private AnnotationNode m_AnnotationNode;

    //Object pool of all comment panels that are going to be used in the CommentUI
    private ObjectPool<CommentPanel> objectPool = new ObjectPool<CommentPanel>(5, 5);
    #endregion

    #region Abstract Methods
    public abstract void Open();
    public abstract void Close();
    #endregion  

    public void InitAnnotationPanel(AnnotationNode annotationNode)
    {
        m_CommentPanels.Capacity = annotationNode.ThreadCount;

        foreach (Comment comment in annotationNode.Replies)
        {
            CommentPanel commentPanel = GetCommentPanelFromPool();
            commentPanel.InitCommentPanel(comment);
            m_CommentPanels.Add(commentPanel);
        }

    }

    public void ChangeComment(CommentPanel panelToEdit)
    {

    }

    public CommentPanel GetCommentPanelFromPool()
    {
        return objectPool.GetObjectFromPool();       
    }

    /// <summary>
    /// Deletes a specific comment out of the thread.
    /// </summary>
    /// <param name="comment"></param>
    public void DeleteComment(Comment comment)
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
    private void DeleteThread()
    {
        m_AnnotationNode.DeleteAnnotation();
        foreach(CommentPanel commentPanel in m_CommentPanels)
        {
            commentPanel.gameObject.SetActive(false);
        }
    }
}
