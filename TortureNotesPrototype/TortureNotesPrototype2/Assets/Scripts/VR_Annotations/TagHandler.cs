using NS_Annotation.NS_Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum TagHandlerMode
{
    Regular,
    Edit,
    Create,
    Delete
}

[System.Serializable]
public struct TagHandlerToggles
{
    public Toggle createToggle;
    public Text createText;

    public Toggle editToggle;
    public Text editText;

    public Toggle deleteToggle;
    public Text deleteText;
}

[RequireComponent(typeof(LineRenderer),typeof( ScaleRect))]
public class TagHandler : VRGrabbable
{
    [SerializeField]
    Transform startOfLine;

    [SerializeField]
    Text titleText;

    [SerializeField]
    Text numberOfAnnotationsText;

   // [SerializeField]
  //  Text descriptionText;

    [SerializeField]
    CommentHandlerSO commentHandlerSO;

    [SerializeField]
    Transform target;

    //[SerializeField]
    //ScaleRect descriptionScaleRect;

    [SerializeField]
    ScaleRect annotationListViewScaler;

    [SerializeField]
    GameObject AnnotationNodePrefab;

    [SerializeField]
    TagHandlerToggles toggles;

    ScaleRect tagHandlerScaleRect;

    LineRenderer lineRenderer;
    public Tag TagData { get; private set; }

    ObjectPool<NodeComponent> annotationNodePool;

    bool annotationsVisible;
    bool viewingAnnotationComments = false;


    public TagHandlerMode tagHandlerMode;

    System.Action<TagHandler, bool> callBackOnToggleAnnotationView;

    System.Action<Tag> callbackWhenClicked;

    private void Awake()
    {
        tagHandlerMode = TagHandlerMode.Regular;

        annotationNodePool = new ObjectPool<NodeComponent>(5, 5, AnnotationNodePrefab);
        lineRenderer = GetComponent<LineRenderer>();
        tagHandlerScaleRect = GetComponent<ScaleRect>();
    }

    public void SetCallBackOnClicked(System.Action<Tag> callBack)
    {
        callbackWhenClicked = callBack;
    }

    public void SetTagHandlerToggles(TagHandlerToggles tagHandlerToggles)
    {
        toggles = tagHandlerToggles;
    }

    public void EnableTagHandler(bool isEnabled)
    {
        tagHandlerScaleRect.StartScaleRect(isEnabled);
        lineRenderer.enabled = isEnabled;
        toggles.createToggle.isOn = false;
        toggles.editToggle.isOn = false;
        toggles.deleteToggle.isOn = false;
    }

    public void PlaceTag(Transform aTarget,Tag aTagData, System.Action<TagHandler,bool> aCallBackOnToggleAnnotationView)
    {
        callBackOnToggleAnnotationView = aCallBackOnToggleAnnotationView;

        TagData = aTagData;
        lineRenderer.enabled = false;
        tagHandlerScaleRect.SetToMinScale();

        transform.position = aTarget.position + aTagData.localPos;
       // descriptionText.text = aTagData.description;
        titleText.text = aTagData.title;
        numberOfAnnotationsText.text = aTagData.AnnotationCount.ToString();

        target = aTarget;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        if (target)
            lineRenderer.SetPositions(new Vector3[] { startOfLine.position, target.position });
    }

    //public override void OnHoverEnter(VRInteractionData vrInteraction)
    //{
    //    if (GalleryLoader.lockView == true)
    //        return;

    //    descriptionScaleRect.ScaleRectToMax();
    //}

    //public override void OnHoverExit(VRInteractionData vrInteraction)
    //{
    //    if (GalleryLoader.lockView == true)
    //        return;

    //    descriptionScaleRect.ScaleRectToMin();
    //}

    public override void OnClick(VRInteractionData vrInteractionData)
    {
        base.OnClick(vrInteractionData);

        callbackWhenClicked(TagData);
    }

    public void ToggleAnnotationView()
    {
        callBackOnToggleAnnotationView(this, !annotationsVisible);
    }

    public void MaximizeAnnotationView()
    {
        annotationsVisible = true;

        for (int i = 0; i < TagData.annotationNodes.Count; i++)
        {
            NodeComponent nodeComponent = annotationNodePool.GetObjectFromPool();
            nodeComponent.InitNodeComponent(TagData.annotationNodes[i], target, this);
        }
    }

    public void MinimizeAnnotationView(System.Action callBackWhenDoneMinimize)
    {
        annotationsVisible = false;

        if (viewingAnnotationComments == false)
        {
            annotationNodePool.ReturnAllActiveToPool();
            if (callBackWhenDoneMinimize != null)
                callBackWhenDoneMinimize();
        }
        else
        {
            annotationNodePool.ReturnAllActiveToPool();
            CloseViewAnnotation(callBackWhenDoneMinimize);
            viewingAnnotationComments = false;
        }

    }

    public void ViewAnnotationComments(NodeComponent aNodeComponent, Transform target)
    {
        if (tagHandlerMode != TagHandlerMode.Regular)
            return;

        viewingAnnotationComments = !viewingAnnotationComments;

        if (viewingAnnotationComments)
        {

            annotationNodePool.ReturnAllActiveToPool(aNodeComponent);

            commentHandlerSO.commentHandler.transform.parent = transform;
            commentHandlerSO.commentHandler.transform.localPosition = Vector3.zero + Vector3.down * 2f;
            commentHandlerSO.commentHandler.transform.localRotation = Quaternion.identity;

            commentHandlerSO.commentHandler.GetComponent<ScaleRect>().ScaleRectToMax();
            commentHandlerSO.commentHandler.InitAnnotationPanel(aNodeComponent.annotationNode);
            commentHandlerSO.commentHandler.Open();
        }
        else
        {
            aNodeComponent.gameObject.SetActive(false);
            CloseViewAnnotation();
            ToggleAnnotationView();
        }
    }

    void CloseViewAnnotation(System.Action callBackWhenDoneMinimize = null)
    {
        commentHandlerSO.commentHandler.GetComponent<ScaleRect>().ScaleRectToMin(callBackWhenDoneMinimize);
        commentHandlerSO.commentHandler.Close();     
    }

    public void CreateAnnotationThread(bool toggleCreateMode)
    {
        if(tagHandlerMode != TagHandlerMode.Regular && toggleCreateMode == true)
        {
            toggles.createToggle.isOn = false;
            return;
        }

        if (toggleCreateMode)
        {

            if (annotationsVisible)
                ToggleAnnotationView();

            GalleryLoader.lockView = true;

            NodeComponent nodeComponent = annotationNodePool.GetObjectFromPool();
            AnnotationNode annotationNode = new AnnotationNode("", System.DateTime.Today.ToShortTimeString(), "");
            annotationNode.AnnotationScale = 1;
            annotationNode.AnnotationEndPos = Vector3.up * 2  + Vector3.right;
            annotationNode.AnnotationStartPos = Vector3.up;
            TagData.annotationNodes.Add(annotationNode);

            nodeComponent.InitNodeComponent(annotationNode, target, this);
            nodeComponent.ScaleThread(true);
            nodeComponent.ToggleEditableText(true);
            nodeComponent.ToggleEditPlacement(true);
            toggles.createText.text = "Place";
        }
        else if (tagHandlerMode == TagHandlerMode.Create)
        {
            NodeComponent[] nodeComponents = annotationNodePool.GetActiveObjects();

            for (int i = 0; i < nodeComponents.Length; i++)
            {
                nodeComponents[i].SaveData();
                nodeComponents[i].ToggleEditableText(false);
            }

            TurnOffCreateMode();
        }

        ChangeTagHandlerMode(TagHandlerMode.Create, toggleCreateMode);
    }

    public void DeleteThread(bool toggleDeleteMode)
    {
        if (tagHandlerMode == TagHandlerMode.Edit && toggleDeleteMode == true)
        {
            toggles.deleteToggle.isOn = false;
            return;
        }

        if (toggleDeleteMode)
        {

            if (tagHandlerMode == TagHandlerMode.Create)
            {
                NodeComponent[] nodeComponents = annotationNodePool.GetActiveObjects();

                for (int i = 0; i < nodeComponents.Length; i++)
                {
                    TagData.annotationNodes.Remove(nodeComponents[i].annotationNode);
                }

                annotationNodePool.ReturnAllActiveToPool();

                toggles.deleteToggle.isOn = false;

                toggleDeleteMode = false;

                TurnOffCreateMode();
            }
            else
            {
                if (annotationsVisible == false)
                    ToggleAnnotationView();

                NodeComponent[] nodeComponents = annotationNodePool.GetActiveObjects();

                for (int i = 0; i < nodeComponents.Length; i++)
                {
                    nodeComponents[i].ScaleThread(true);
                }
            }

            GalleryLoader.lockView = true;
        }
        else if(tagHandlerMode == TagHandlerMode.Delete)
        {
            GalleryLoader.lockView = false;
        }



        ChangeTagHandlerMode(TagHandlerMode.Delete, toggleDeleteMode);
    }

    public void EditThread(bool toggleEditMode)
    {
        if (tagHandlerMode != TagHandlerMode.Regular && toggleEditMode == true)
        {
            toggles.editToggle.isOn = false;
            return;
        }

        if (toggleEditMode)
        {
           // descriptionScaleRect.ScaleRectToMax();

            GalleryLoader.lockView = true;
            ToggleEditTag(true);

            NodeComponent[] nodeComponents = annotationNodePool.GetActiveObjects();

            for (int i = 0; i < nodeComponents.Length; i++)
            {
                nodeComponents[i].ScaleThread(true);
                nodeComponents[i].ToggleEditableText(true);
                nodeComponents[i].ToggleEditPlacement(true);
            }
        }
        else if (tagHandlerMode == TagHandlerMode.Edit)
        {
           // descriptionScaleRect.ScaleRectToMin();

            GalleryLoader.lockView = false;
            ToggleEditTag(false);

            NodeComponent[] nodeComponents = annotationNodePool.GetActiveObjects();

            for (int i = 0; i < nodeComponents.Length; i++)
            {
                nodeComponents[i].ScaleThread(false);
                nodeComponents[i].ToggleEditableText(false);
                nodeComponents[i].ToggleEditPlacement(false);
                nodeComponents[i].SaveData();
            }
        }

        ChangeTagHandlerMode(TagHandlerMode.Edit, toggleEditMode);
    }

    void ToggleEditTag(bool isEditable)
    {
        titleText.GetComponent<VREditableText>().enabled = isEditable;
       // descriptionText.GetComponent<VREditableText>().enabled = isEditable;
        grabEnabled = isEditable;

        if(isEditable == false)
        {
            TagData.title = titleText.text;
          //  TagData.description = descriptionText.text;
            TagData.localPos = target.InverseTransformPoint(transform.position);
        }
    }

    void ChangeTagHandlerMode(TagHandlerMode handlerMode, bool toggleOn)
    {
        if(toggleOn)
            tagHandlerMode = handlerMode;
        else if(tagHandlerMode == handlerMode)
            tagHandlerMode = TagHandlerMode.Regular;
    }

    public void DeleteThread(NodeComponent nodeComponent)
    {
        TagData.annotationNodes.Remove(nodeComponent.annotationNode);
        nodeComponent.gameObject.SetActive(false);
        numberOfAnnotationsText.text = TagData.AnnotationCount.ToString();
    }

    void TurnOffCreateMode()
    {
        GalleryLoader.lockView = false;

        toggles.createText.text = "Create";
        toggles.createToggle.isOn = false;
        numberOfAnnotationsText.text = TagData.AnnotationCount.ToString();
        annotationNodePool.ReturnAllActiveToPool();
    }


}
