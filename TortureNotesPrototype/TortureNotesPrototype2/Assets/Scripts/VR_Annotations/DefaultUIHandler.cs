using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultUIHandler : CommentUIHandler
{
    [SerializeField]
    private GameObject ScrollViewContent;

    public override void Close()
    {
        
    }

    public override void Open()
    {

    }

    public override void AddCommentToUI()
    {
        CommentPanel obj = GetCommentPanelFromPool();
        obj.gameObject.SetActive(true);
        obj.transform.SetParent(ScrollViewContent.transform);
        obj.transform.localScale = new Vector3(1, 1, 1);

        AddCommentToAnnotationNode(new NS_Annotation.NS_Data.Comment(obj.authorText.text, obj.dateText.text, obj.contentText.text));
    }
}
