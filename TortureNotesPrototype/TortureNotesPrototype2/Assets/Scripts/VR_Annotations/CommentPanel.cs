using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CommentPanel : MonoBehaviour
{
    [SerializeField]
    Text authorText;
    [SerializeField]
    Text dateText;
    [SerializeField]
    Text contentText;

    List<CommentPanel> commentPanelChildren;

    //called recursivily
    public void InitCommentPanel(Comment comment)
    {
        authorText.text = comment.author;
        dateText.text = comment.date;
        contentText.text = comment.date;

        commentPanelChildren.Capacity = comment.m_replies.Count;

        for (int i = 0; i < comment.m_replies.Count; i++)
        {
            CommentPanel commentChild = CommentUIHandler.GetCommentPanelFromPool();
            commentChild.InitCommentPanel(comment.m_replies[i]);
            commentPanelChildren.Add(commentChild);
        }      
    }

    public void EditComment()
    {

    }

    public void DeleteComment()
    {

    }
	
    public void AddReply()
    {

    }

    public void ToggleChildExpand()
    {

    }

    //called recursivily
    public void ClearChildren()
    {
        for (int i = 0; i < commentPanelChildren.Count; i++)
            commentPanelChildren[i].ClearChildren();

        gameObject.SetActive(false);
    }
}
