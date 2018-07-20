using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRGrabbable : VRInteractable
{
    [SerializeField]
    float rotSpeed = 0;
    [SerializeField]
    float reelInSpeed = 0;

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

    public override void OnClickHeld(VRInteractionData vrInteraction)
    {
        if (rotSpeed > 0 && vrInteraction.secondaryClickPressed == false)
        {
            transform.Rotate(vrInteraction.handTrans.right, rotSpeed * Time.deltaTime * vrInteraction.movementDirection.y, Space.World);
            transform.Rotate(vrInteraction.handTrans.up, -rotSpeed * Time.deltaTime * vrInteraction.movementDirection.x, Space.World);
        }
        else if (vrInteraction.secondaryClickPressed)
        {
            transform.position = transform.position - vrInteraction.handTrans.forward * reelInSpeed * -vrInteraction.movementDirection.y;
        }

    }
}
