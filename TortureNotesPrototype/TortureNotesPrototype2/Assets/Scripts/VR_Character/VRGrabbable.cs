using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRGrabbable : VRInteractable
{
    override public void OnClick(VRInteractionData vrInteractionData)
    {
        if (transform.parent == null || transform.parent != vrInteractionData.handTrans)
            transform.parent = vrInteractionData.handTrans;
    }

    override public void OnClickRelease(VRInteractionData vrInteractionData)
    {
        if (transform.parent == vrInteractionData.handTrans)
            transform.parent = null;
    }
}
