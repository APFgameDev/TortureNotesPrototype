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
    const float SELECTABLE_OUTLINE_WIDTH = 0.05f;

    private Transform originalParent;
    protected bool m_grabed = false;
    private MeshRenderer meshRenderer;

    short hoverCounts = 0;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        originalParent = transform.parent;
    }

    override public void OnClick(VRInteractionData vrInteractionData)
    {
        base.OnClick(vrInteractionData);

        if (m_grabed == true)
            return;

        GrabObject(vrInteractionData.handTrans);
    }

    override public void OnClickRelease(VRInteractionData vrInteractionData)
    {
        base.OnClickRelease(vrInteractionData);

        if (transform.parent == vrInteractionData.handTrans)
        {
            transform.SetParent(originalParent);
            m_grabed = false;
        }
    }

    public override void OnClickHeld(VRInteractionData vrInteraction)
    {
        base.OnClickHeld(vrInteraction);

        if (rotSpeed > 0 && vrInteraction.secondaryClickPressed == false)
        {
            transform.position = transform.position - vrInteraction.handTrans.forward * reelInSpeed * -vrInteraction.movementDirection.y;
            transform.localScale = transform.localScale + Vector3.one * scaleSpeed * -vrInteraction.movementDirection.x;
        }

        else if (vrInteraction.secondaryClickPressed)
        {
            transform.Rotate(vrInteraction.handTrans.right, rotSpeed * Time.deltaTime * vrInteraction.movementDirection.y, Space.World);
            transform.Rotate(vrInteraction.handTrans.up, -rotSpeed * Time.deltaTime * vrInteraction.movementDirection.x, Space.World);
        }
    }

    public override void OnHoverEnter(VRInteractionData vrInteraction)
    {
        hoverCounts++;

        if (hoverCounts == 1)
        {
            base.OnHoverEnter(vrInteraction);
        }
    }

    public override void OnHoverExit(VRInteractionData vrInteraction)
    {
        hoverCounts--;

        if (hoverCounts == 0)
        {
            base.OnHoverExit(vrInteraction);
        }
    }

    public void SetSpeeds(float aRotSpeed, float aReelInSpeed, float aScaleSpeed)
    {
        rotSpeed = aRotSpeed;
        reelInSpeed = aReelInSpeed;
        scaleSpeed = aScaleSpeed;
    }

    public void SetOriginalParent(Transform parent)
    {
        originalParent = parent;
    }

    public void GrabObject(Transform handTrans)
    {
        transform.SetParent(handTrans);
        m_grabed = true;
    }
}
