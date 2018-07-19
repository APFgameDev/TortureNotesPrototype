using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NS_Annotation.NS_Data;


[RequireComponent(typeof(Text))]
public class CommentPanel : MonoBehaviour
{
    [SerializeField]
    private Text authorText;
    [SerializeField]
    private Text dateText;
    [SerializeField]
    private Text contentText;

    private Comment m_MyComment;

    //called recursivily
    public void InitCommentPanel(Comment comment)
    {    
        authorText.text = comment.author;
        dateText.text = comment.date;
        contentText.text = comment.date;

        m_MyComment = comment;
    }
}
