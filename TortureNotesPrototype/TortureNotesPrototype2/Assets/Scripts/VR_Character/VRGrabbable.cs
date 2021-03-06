﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class VRGrabbable : VRInteractable
{
    [SerializeField]
    float rotSpeed = 0;
    [SerializeField]
    float reelInSpeed = 0;
    [SerializeField]
    float scaleSpeed = 0;

    Transform originalParent;

    public bool grabEnabled = true;

    private void OnEnable()
    {
        originalParent = transform.parent;
    }

    override public void OnClick(VRInteractionData vrInteractionData)
    {
        base.OnClick(vrInteractionData);

        if (grabEnabled == false)
            return;

        if (transform.parent == null || transform.parent != vrInteractionData.handTrans)
            transform.parent = vrInteractionData.handTrans;
    }

    override public void OnClickRelease(VRInteractionData vrInteractionData)
    {
        base.OnClickRelease(vrInteractionData);

        if (grabEnabled == false)
            return;

        if (transform.parent == vrInteractionData.handTrans)
            transform.parent = originalParent;
    }

    public override void OnClickHeld(VRInteractionData vrInteraction)
    {
        base.OnClickHeld(vrInteraction);

        if (grabEnabled == false)
            return;

        if (rotSpeed > 0 && vrInteraction.secondaryClickPressed == false)
        {
            transform.Rotate(vrInteraction.handTrans.right, rotSpeed * Time.deltaTime * vrInteraction.movementDirection.y, Space.World);
            transform.Rotate(vrInteraction.handTrans.up, -rotSpeed * Time.deltaTime * vrInteraction.movementDirection.x, Space.World);
        }
        else if (vrInteraction.secondaryClickPressed)
        {
            transform.position = transform.position - vrInteraction.handTrans.forward * reelInSpeed * -vrInteraction.movementDirection.y;
            transform.localScale = transform.localScale + Vector3.one * scaleSpeed * -vrInteraction.movementDirection.x;
        }
    }

    public void SetSpeeds(float aRotSpeed, float aReelInSpeed, float aScaleSpeed)
    {
        rotSpeed = aRotSpeed;
        reelInSpeed = aReelInSpeed;
        scaleSpeed = aScaleSpeed;
    }
}
