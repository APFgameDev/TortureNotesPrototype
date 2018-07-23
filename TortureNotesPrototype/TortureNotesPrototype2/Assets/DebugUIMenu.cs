using NS_Annotation.NS_Data;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIMenu : MonoBehaviour
{
    public Text AnnotUserName;
    public Text AnnotContent;
    public CommentHandlerSO CommentHandlerSO;

    public Text ReplyUserName;
    public Text ReplyContent;

    public void CreateAnnotationDataSet()
    {

        System.DateTime date = System.DateTime.Today;
        string value = date.Year.ToString() + " / " + date.Month.ToString() + " / "+ date.Day.ToString();

        AnnotationNode annotationNode = new AnnotationNode(AnnotUserName.text, value, AnnotContent.text);
        CommentHandlerSO.commentHandler.InitAnnotationPanel(annotationNode, null);
    }

    public void CreateReplyToAnnotation()
    {
        System.DateTime date = System.DateTime.Today;
        string value = date.Year.ToString() + " / " + date.Month.ToString() + " / " + date.Day.ToString();

        Comment comment = ReplyUserName.text;
    }
}
