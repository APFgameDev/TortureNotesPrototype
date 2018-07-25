using NS_Annotation.NS_Data;
using UnityEngine.UI;
using UnityEngine;

public class DefaultUIHandler : CommentUIHandler
{
    [SerializeField]
    private GameObject ScrollViewContent;

    [SerializeField]
    private Text name;
    [SerializeField]
    private Text content;
    [SerializeField]
    private Toggle[] priorities;

    [SerializeField]
    private ColorSO lowPriority;
    [SerializeField]
    private ColorSO normalPriority;
    [SerializeField]
    private ColorSO highPriority;

    public override void Close()
    {
       // m_CloseCallback();
    }

    public override void Open()
    {

    }

    public override void AddCommentToUI(Comment comment)
    {
        CommentPanel obj = GetCommentPanelFromPool();
        obj.InitCommentPanel(comment);
        obj.gameObject.SetActive(true);
        obj.transform.SetParent(ScrollViewContent.transform);
        switch (obj.Comment.priority)
        {
            case Priority.Low:
                obj.priorityImage.color = lowPriority.color;
                break;

            case Priority.Medium:
                obj.priorityImage.color = normalPriority.color;
                break;

            case Priority.High:                
                obj.priorityImage.color = highPriority.color;
                break;

            default:
                obj.priorityImage.color = normalPriority.color;
                break;
        }

        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    public void CreateReplyToAnnotation()
    {
        System.DateTime date = System.DateTime.Today;
        string value = date.Year.ToString() + " / " + date.Month.ToString() + " / " + date.Day.ToString();

        Priority priority = Priority.Low;

        for (int i = 0; i < priorities.Length; i++)
        {
            if(priorities[i].isOn)
            {
                priority = (Priority)i + 1;
                break;
            }
        }

        Comment comment = new Comment(name.text, value, content.text, priority);

        AddCommentToUI(comment);
    }
}
