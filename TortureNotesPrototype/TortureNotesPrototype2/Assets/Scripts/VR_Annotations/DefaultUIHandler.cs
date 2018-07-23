using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultUIHandler : CommentUIHandler
{
    [SerializeField]
    private GameObject ScrollViewContent;

    public override void Close()
    {
        m_CloseCallback();
    }

    public override void Open()
    {

    }

    public override void AddCommentToUI(NS_Annotation.NS_Data.Comment comment)
    {
        CommentPanel obj = GetCommentPanelFromPool();
        obj.InitCommentPanel(comment);
        obj.gameObject.SetActive(true);
        obj.transform.SetParent(ScrollViewContent.transform);
        obj.transform.localScale = new Vector3(1, 1, 1);        
    }
}
