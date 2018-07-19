using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NS_Annotation.NS_Data;

public class CommentPanel : MonoBehaviour
{    
    public Text authorText;
    public Text dateText;
    public Text contentText;

    private Comment m_MyComment;
    private CommentUIHandler m_UIManager;

    //called recursivily
    public void InitCommentPanel(Comment comment, CommentUIHandler baseHandler)
    {    
        authorText.text = comment.author;
        dateText.text = comment.date;
        contentText.text = comment.date;

        m_MyComment = comment;
        m_UIManager = baseHandler;
    }

    public void EditComment()
    {
        m_UIManager.ChangeContent(this);
    }

    public void DeleteComment()
    {
        m_UIManager.DeleteComment(m_MyComment);
    }
}
