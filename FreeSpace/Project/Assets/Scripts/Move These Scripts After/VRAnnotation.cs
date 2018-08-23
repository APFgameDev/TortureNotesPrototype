using Annotation.SO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VRAnnotation : VRGrabbable
{
    [SerializeField]
    private LineRenderer m_LineRenderer;
    [SerializeField]
    Transform m_highlightNode;

    [SerializeField]
    GameObject commentPanelPrefab;

    [SerializeField]
    GameObject contentBox;

    [SerializeField]
    UnityEventInvoker onTurnOnButtons;

    [SerializeField]
    UnityEventInvoker onTurnOffButtons;

    bool m_stopLineUpdate = true;
    bool m_releaseOnClick = false;

    [SerializeField]
    BoxCollider[] boxColliders;

    GameObject m_commentToDelete;

    [SerializeField]
    ScaleRect m_commentDeleteScaleRect;

    [SerializeField]
    float m_minimizeMoveTime = 1;

    Vector3 m_originalPosition;

    [SerializeField]
    Image m_commentImage;

    [SerializeField]
    Sprite m_noCommentSprite;

    [SerializeField]
    Sprite m_yesCommentSprite;

    [SerializeField]
    Text m_commentText;

    int m_numComments = 0;


    short m_numGrabs = 0;

    private void Start()
    {
        m_LineRenderer.positionCount = 2;
    }


    public void StartUp(Vector3 clickPoint, Transform parent, bool releaseOnClick)
    {
        m_highlightNode.position = clickPoint;

        m_highlightNode.GetComponent<ScaleRect>().callsWhenDoneMaxScale.AddListener(() => 
        {
            m_highlightNode.SetParent(parent,true);
        });

        m_releaseOnClick = releaseOnClick;

        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
    }

    public void Remove()
    {
        Destroy(m_highlightNode.gameObject);
        Destroy(transform.parent.gameObject);
    }

    public override void OnClickRelease(VRInteractionData vrInteraction)
    {
        if (m_releaseOnClick)
        {
            base.OnClickRelease(vrInteraction);
            base.OnSecondaryClickRelease(vrInteraction);

            EndHold(vrInteraction);
            vrInteraction.m_laser.ForceRelease();
        }
    }

    public void CreateAnnotation()
    {
        GameObject newComment = Instantiate(commentPanelPrefab, contentBox.transform);
        newComment.transform.localPosition = Vector3.zero;
        newComment.transform.localRotation = Quaternion.identity;

        AnnotationComment annotationComment = newComment.GetComponent<AnnotationComment>();
        annotationComment.DeleteCommentCallback = OnOpenDeleteCommentPanel;

        newComment.gameObject.SetActive(true);  
        newComment.GetComponentInChildren<VRText>().StartListening();

        if (m_numComments == 0)
        {
            m_commentImage.sprite = m_yesCommentSprite;
        }

        m_numComments++;

        m_commentText.text = m_numComments.ToString();
    }

    public void TurnOnButtons()
    {
        onTurnOnButtons.Invoke();
        m_highlightOn = true;
    }

    public void TurnOffButtons()
    {
        onTurnOffButtons.Invoke();
        m_highlightOn = false;
    }

    private void Update()
    {
        UpdateLineRendererPosition();
    }

    public void UpdateLineRendererPosition()
    {
        Vector3 lineStartPos = Vector3.zero;
        float distance = float.MaxValue;
        for (int i = 0; i < boxColliders.Length; i++)
        {
            Vector3 closestPoint = boxColliders[i].ClosestPoint(m_highlightNode.position);

            float sqrMag = (closestPoint - m_highlightNode.position).sqrMagnitude;

            if (sqrMag < distance)
            {
                distance = sqrMag;
                lineStartPos = closestPoint;
            }
        }
            

        m_LineRenderer.SetPositions(new Vector3[] { lineStartPos, m_highlightNode.position });
    }

    public void OnOpenDeleteCommentPanel(GameObject gameObject)
    {
        m_commentToDelete = gameObject;

        m_commentDeleteScaleRect.StartScaleRect(true);
    }

    public void OnActuallyDeleteComment()
    {
        Destroy(m_commentToDelete);

        m_numComments--;



        if (m_numComments == 0)
        {
            m_commentImage.sprite = m_noCommentSprite;
            m_commentText.text = "";
        }
        else
        {
            m_commentText.text = m_numComments.ToString();
        }

    }

    IEnumerator MoveToHighlight()
    {
        m_originalPosition = transform.position;

        float time = 0;

        while(time < m_minimizeMoveTime)
        {
            transform.position = Vector3.Lerp(m_originalPosition, m_highlightNode.position, time / m_minimizeMoveTime);

            yield return null;

            time += Time.deltaTime;
        }

    }


    IEnumerator MoveToOriginalPosition()
    {
        float time = 0;

        while (time < m_minimizeMoveTime)
        {
            transform.position = Vector3.Lerp(m_highlightNode.position, m_originalPosition, time / m_minimizeMoveTime);

            yield return null;

            time += Time.deltaTime;
        }
    }

    public void StartMoveToHighlight()
    {
        StopCoroutine(MoveToHighlight());
        StopCoroutine(MoveToOriginalPosition());

        StartCoroutine(MoveToHighlight());
    }

    public void StartMoveToOriginalPosition()
    {
        StopCoroutine(MoveToHighlight());
        StopCoroutine(MoveToOriginalPosition());

        StartCoroutine(MoveToOriginalPosition());
    }
}
