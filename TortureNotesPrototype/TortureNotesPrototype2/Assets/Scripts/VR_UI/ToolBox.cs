using NS_Annotation.NS_Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(OnEnableSpawnInRelativePositionToPlayerWithOffset), typeof(ScaleRect))]
public class ToolBox : VRGrabbable
{
    VRSelectableObject currentSelectedObject;
    OnEnableSpawnInRelativePositionToPlayerWithOffset resetRelativePos;
    ScaleRect scaleRect;

    [SerializeField]
    Toggle writeToggle;

    [SerializeField]
    TagHandlerToggles handlerToggles;

    //ObjectPool<>

    //GameObject content;

    private void Awake()
    {
        resetRelativePos = GetComponent<OnEnableSpawnInRelativePositionToPlayerWithOffset>();
        scaleRect = GetComponent<ScaleRect>();
    }

    public void SetActive(VRSelectableObject aObjectActivating)
    {
        if (aObjectActivating != currentSelectedObject)
        {
            if (currentSelectedObject != null)
                currentSelectedObject.EnableTagHandler(false);

            grabEnabled = false;

            writeToggle.isOn = false;

            scaleRect.ScaleRectToMin(WhenDoneMinimizing);
            currentSelectedObject = aObjectActivating;
        }  
    }

    void WhenDoneMinimizing()
    {
        scaleRect.ScaleRectToMax(WhenDoneMaximizing);
        resetRelativePos.SetToReposition();
    }

    void OpenAnnotationView(Tag tagData)
    {

    }

    void WhenDoneMaximizing()
    {
        grabEnabled = true;
        currentSelectedObject.SetTagHandlerToggleCallBacks(handlerToggles);
        currentSelectedObject.SetTagHandlerOnClickCallBack(OpenAnnotationView);
    }

    public void OnEnableTag(bool isEnabled)
    {

        if (scaleRect.GetIsCurrentlyScaling() == false)
            currentSelectedObject.EnableTagHandler(isEnabled);
        else
            writeToggle.isOn = !isEnabled;
    }
}
