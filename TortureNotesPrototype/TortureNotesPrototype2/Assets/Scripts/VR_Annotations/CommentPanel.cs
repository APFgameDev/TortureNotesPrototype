using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NS_Annotation.NS_Data;

public class CommentPanel : MonoBehaviour
{
    public CommentHandlerSO CommentHandlerSO;

    public Text authorText;
    public Text dateText;
    public Text contentText;

    private Comment m_MyComment;

    //called recursivily
    public void InitCommentPanel(Comment comment)
    {    
        authorText.text = comment.author;
        dateText.text = comment.date;
        contentText.text = comment.content;

        m_MyComment = comment;
    }

    public void EditComment()
    {
        CommentHandlerSO.commentHandler.ChangeContent(this);
    }

    public void DeleteComment()
    {
        CommentHandlerSO.commentHandler.DeleteComment(this);
    }

    public Comment Comment { get { return m_MyComment; } }
}
