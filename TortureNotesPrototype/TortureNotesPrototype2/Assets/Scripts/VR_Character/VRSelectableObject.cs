using NS_Annotation.NS_Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class VRSelectableObject : VRGrabbable
{
    MeshRenderer meshRenderer;
    float selectableOutlineWidth = 0.05f;
    TagHandler tagHandler;
    ToolBox toolBox;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetTagHandler(TagHandler aTagHandler)
    {
        tagHandler = aTagHandler;
    }

    public void SetTagHandlerToggleCallBacks(TagHandlerToggles tagHandlerToggles)
    {
        tagHandlerToggles.createToggle.onValueChanged.RemoveAllListeners();
        tagHandlerToggles.createToggle.onValueChanged.AddListener(tagHandler.CreateAnnotationThread);

        tagHandlerToggles.deleteToggle.onValueChanged.RemoveAllListeners();
        tagHandlerToggles.deleteToggle.onValueChanged.AddListener(tagHandler.DeleteThread);

        tagHandlerToggles.editToggle.onValueChanged.RemoveAllListeners();
        tagHandlerToggles.editToggle.onValueChanged.AddListener(tagHandler.EditThread);

        tagHandler.SetTagHandlerToggles(tagHandlerToggles);
    }

    public void SetTagHandlerOnClickCallBack(System.Action<Tag> callBack)
    {
        tagHandler.SetCallBackOnClicked(callBack);
    }

    public void SetToolBox(ToolBox aToolBox)
    {
        toolBox = aToolBox;
    }

    public override void OnHoverEnter(VRInteractionData vrInteraction)
    {
        meshRenderer.material.SetFloat("_HighLightSize", selectableOutlineWidth);
        base.OnHoverEnter(vrInteraction);
    }

    public override void OnHoverExit(VRInteractionData vrInteraction)
    {
        meshRenderer.material.SetFloat("_HighLightSize", 0);
        base.OnHoverExit(vrInteraction);
    }

    public override void OnClick(VRInteractionData vrInteractionData)
    {
        toolBox.SetActive(this);
        base.OnClick(vrInteractionData);
    }

    public void EnableTagHandler(bool isEnabled)
    {
        tagHandler.EnableTagHandler(isEnabled);
    }
}
