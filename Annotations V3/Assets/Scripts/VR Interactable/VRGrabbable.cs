using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class VRGrabbable : VRInteractable
{
    [SerializeField]
    float rotSpeed = 60;
    [SerializeField]
    float reelInSpeed = 0.05f;
    [SerializeField]
    float scaleSpeed = 10;
    [SerializeField]
    [Range(0.001f, 5f)]
    float m_minScale = 0.1f;
    [SerializeField]
    [Range(5f, 100f)]
    float m_maxScale = 100;

    private Transform originalParent;
    protected bool m_grabed = false;

    short hoverCounts = 0;
    float m_laserScaleStartDistance;
    Quaternion m_laserRotStartOffset;

    UnityEngine.Events.UnityAction ScaleUpdate;
    UnityEngine.Events.UnityAction SetStartScaleDistance;

    private Vector3 m_StartScale = Vector3.zero;
    private float m_OriginalControllerDistance = 0;


    private void OnEnable()
    {
        originalParent = transform.parent;
    }

    override public void OnSecondaryClick(VRInteractionData vrInteractionData)
    {
        base.OnClick(vrInteractionData);

        if (m_grabed == true)
            return;

        SetStartScaleDistance = delegate
        {
            SetLaserStartScaleDistance(
                vrInteractionData.m_handTrans,
                vrInteractionData.m_laser.GetOtherLaser.transform);
        };

        ScaleUpdate = delegate
        {
            UpdateScale(
                vrInteractionData.m_handTrans,
                vrInteractionData.m_laser.GetOtherLaser.transform);
        };

        vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnPressed.UnityEvent.AddListener(SetStartScaleDistance);

        vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnHeld.UnityEvent.AddListener(ScaleUpdate);

        GrabObject(vrInteractionData.m_handTrans);
    }

    override public void OnSecondaryClickRelease(VRInteractionData vrInteractionData)
    {
        base.OnClickRelease(vrInteractionData);

        if (transform.parent == vrInteractionData.m_handTrans)
        {
            transform.SetParent(originalParent);
            m_grabed = false;

            vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnPressed.UnityEvent.RemoveListener(SetStartScaleDistance);

            vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnHeld.UnityEvent.RemoveListener(ScaleUpdate);
        }
    }

    public override void OnSecondaryClickHeld(VRInteractionData vrInteraction)
    {
        base.OnClickHeld(vrInteraction);

        Vector2 inputAxis = vrInteraction.m_laser.GetThumbAxisValue;

        if (vrInteraction.m_laser.GetTriggerEvents.inputPressed.Value == false)
        {
            if (Vector3.Distance(vrInteraction.m_handTrans.position, transform.position) < vrInteraction.m_laser.GetMaxLaserDistance || inputAxis.y < 0)
                transform.position = transform.position + vrInteraction.m_handTrans.forward * reelInSpeed * inputAxis.y;
        }
        else
        {
            transform.Rotate(vrInteraction.m_handTrans.right, rotSpeed * Time.deltaTime * inputAxis.y, Space.World);
            transform.Rotate(vrInteraction.m_handTrans.up, -rotSpeed * Time.deltaTime * inputAxis.x, Space.World);
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

    public override WhatIsHold GetWhatIsHold()
    {
        return WhatIsHold.Secondary;
    }

    public void UpdateScale(Transform laser, Transform otherLaser)
    {
        float distance = Vector3.Distance(laser.position, otherLaser.position);
        transform.localScale = m_StartScale * (distance / m_OriginalControllerDistance);
    }

    public void SetLaserStartScaleDistance(Transform laser, Transform otherLaser)
    {
        m_StartScale = transform.localScale;
        m_OriginalControllerDistance = Vector3.Distance(laser.position, otherLaser.position);
    }

    Quaternion GetControllerOrientation(Transform laser, Transform otherLaser)
    {
        Vector3 direction = otherLaser.position - laser.position;
        Vector3 up = (laser.forward + otherLaser.forward) / 2.0f;
        return Quaternion.LookRotation(direction, up);
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
