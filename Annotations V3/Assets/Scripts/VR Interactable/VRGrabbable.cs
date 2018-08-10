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
    [SerializeField]
    float scaleSpeed = 0;
    [SerializeField]
    [Range(0.001f, 5f)]
    float m_minScale;
    [SerializeField]
    [Range(5f, 100f)]
    float m_maxScale;

    const float SELECTABLE_OUTLINE_WIDTH = 0.05f;

    private Transform originalParent;
    protected bool m_grabed = false;
    private MeshRenderer meshRenderer;

    short hoverCounts = 0;
    float m_laserScaleStartDistance;

    UnityEngine.Events.UnityAction ScaleUpdate;
    UnityEngine.Events.UnityAction SetStartScaleDistance;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

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
                vrInteractionData.m_handTrans.position,
                vrInteractionData.m_laser.GetOtherLaser.transform.position);
        };

        ScaleUpdate = delegate
        {
            UpdateScale(
                vrInteractionData.m_handTrans.position,
                vrInteractionData.m_laser.GetOtherLaser.transform.position);
        };

        vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnPressed.UnityEvent.AddListener (SetStartScaleDistance);

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


            vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnPressed.UnityEvent.RemoveListener (SetStartScaleDistance);

            vrInteractionData.m_laser.GetOtherLaser.GetGripEvents.OnHeld.UnityEvent.RemoveListener (ScaleUpdate);

        }
    }

    public override void OnSecondaryClickHeld(VRInteractionData vrInteraction)
    {
        base.OnClickHeld(vrInteraction);

        Vector2 inputAxis = vrInteraction.m_laser.GetThumbAxisValue;

        if (vrInteraction.m_laser.GetTriggerEvents.inputPressed.Value == false)
        {
            if(Vector3.Distance( vrInteraction.m_handTrans.position, transform.position) < vrInteraction.m_laser.GetMaxLaserDistance )
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

    public void UpdateScale(Vector3 laserPos, Vector3 otherLaserPos)
    {
        float currentScaleDistance = Vector3.Distance(laserPos, otherLaserPos);
        transform.localScale = Vector3.ClampMagnitude( Vector3.Max( transform.localScale + Vector3.one * (currentScaleDistance - m_laserScaleStartDistance) * scaleSpeed, Vector3.one * m_minScale), m_maxScale);
        m_laserScaleStartDistance = currentScaleDistance;
    }

    public void SetLaserStartScaleDistance(Vector3 laserPos, Vector3 otherLaserPos)
    {
        m_laserScaleStartDistance = Vector3.Distance(laserPos, otherLaserPos);
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
