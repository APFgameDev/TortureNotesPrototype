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
    [SerializeField]
    bool m_scaleable = true;
    [SerializeField]
    bool m_clampScale = true;

    [SerializeField]
    MeshRenderer selectionMesh;
    [SerializeField]
    [Range(0.001f, 100)]
    protected float highlightSelectionWidth = 0.02f;



    private Transform originalParent;
    protected bool m_grabed = false;
    public bool m_highlightOn = true;


    short hoverCounts = 0;
    float m_laserScaleStartDistance;
    Quaternion m_laserRotStartOffset;

    UnityEngine.Events.UnityAction ScaleUpdate;
    UnityEngine.Events.UnityAction SetStartScaleDistance;


    private void OnEnable()
    {
        originalParent = transform.parent;
    }

    override public void OnSecondaryClick(VRInteractionData vrInteractionData)
    {
        base.OnSecondaryClick(vrInteractionData);

        StartHold(vrInteractionData);
    }

    override public void OnSecondaryClickRelease(VRInteractionData vrInteractionData)
    {
        base.OnSecondaryClickRelease(vrInteractionData);

        EndHold(vrInteractionData);
    }

    public override void OnSecondaryClickHeld(VRInteractionData vrInteraction)
    {
        base.OnSecondaryClickHeld(vrInteraction);

        Vector2 inputAxis = vrInteraction.m_laser.GetThumbAxisValue;

        if (vrInteraction.m_laser.GetTriggerEvents.inputPressed.Value == false)
        {
            if(Vector3.Distance( vrInteraction.m_handTrans.position, transform.position) < vrInteraction.m_laser.GetMaxLaserDistance || inputAxis.y < 0 )
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

            if (m_highlightOn)
                SetHighlightSizeToMax();
        }
    }

    public override void OnHoverExit(VRInteractionData vrInteraction)
    {
        hoverCounts--;

        if (hoverCounts == 0)
        {
            base.OnHoverExit(vrInteraction);

            if (m_highlightOn)
                SetHighlightSizeToMin();
        }
    }

    public override WhatIsHold GetWhatIsHold()
    {
        return WhatIsHold.Secondary;
    }

    public void UpdateScale(Transform laser, Transform otherLaser)
    {
        float currentScaleDistance = Vector3.Distance(laser.position, otherLaser.position);
        if (m_clampScale)
            transform.localScale = Vector3.ClampMagnitude(Vector3.Max(transform.localScale + Vector3.one * (currentScaleDistance - m_laserScaleStartDistance) * scaleSpeed, Vector3.one * m_minScale), m_maxScale);
        else
            transform.localScale = transform.localScale + Vector3.one * (currentScaleDistance - m_laserScaleStartDistance) * scaleSpeed;

        m_laserScaleStartDistance = currentScaleDistance;
        transform.rotation = Quaternion.Slerp(transform.rotation, GetControllerOrientation(laser,otherLaser) * m_laserRotStartOffset, Time.deltaTime * 20.0f);
    }

    public void SetLaserStartScaleDistance(Transform laser, Transform otherLaser)
    {
        m_laserScaleStartDistance = Vector3.Distance(laser.position, otherLaser.position);
        m_laserRotStartOffset = Quaternion.Inverse(GetControllerOrientation(laser,otherLaser)) * transform.rotation;
    }

    public void SetHighlightSizeToMax()
    {
        if (selectionMesh != null)
            selectionMesh.material.SetFloat("_HighLightSize", highlightSelectionWidth);
    }

    public void SetHighlightSizeToMin()
    {
        if (selectionMesh != null)
            selectionMesh.material.SetFloat("_HighLightSize", 0);
    }

    protected void StartHold(VRInteractionData vrInteractionData)
    {
        if (m_grabed == true)
            return;

        if (m_scaleable)
        {
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
        }

        GrabObject(vrInteractionData.m_handTrans);
    }

    protected void EndHold(VRInteractionData vrInteractionData)
    {
        if (transform.parent == vrInteractionData.m_handTrans)
        {
            transform.SetParent(originalParent);
            m_grabed = false;

            if (m_scaleable)
            {
                vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnPressed.UnityEvent.RemoveListener(SetStartScaleDistance);
                vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnHeld.UnityEvent.RemoveListener(ScaleUpdate);
            }
        }
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
