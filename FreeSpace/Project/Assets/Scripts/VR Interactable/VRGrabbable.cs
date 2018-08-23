using Annotation;
using cakeslice;
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


    private Transform originalParent;
    protected bool m_grabed = false;

    public bool m_isGrabbable = true;

    public bool m_IsGrabable { get {return m_isGrabbable; } set { m_isGrabbable = value; } }
    public bool m_highlightOn = true;

    short hoverCounts = 0;
    float m_laserScaleStartDistance;
    Quaternion m_laserRotStartOffset;

    [SerializeField]
    protected Outline m_outliner;

    UnityEngine.Events.UnityAction ScaleUpdate;
    UnityEngine.Events.UnityAction SetStartScaleDistance;
    [SerializeField]
    Rigidbody m_rigidbody;

    Vector3 velo;
    Vector3 oldPos;

    Vector3 startPos;
    Vector3 startScale;
    Quaternion startRot;
    const float killY = -10;

    [SerializeField]
    bool m_keepKinematic = false;

    private void OnEnable()
    {
        originalParent = transform.parent;

        if (m_outliner == null)
            m_outliner = GetComponent<Outline>();



        if(m_rigidbody == null)
            m_rigidbody = GetComponent<Rigidbody>();

        m_outliner.enabled = false;

        startPos = transform.position;
        startScale = transform.localScale;
        startRot = transform.rotation;
    }

    public override void OnClickHeld(VRInteractionData vrInteraction)
    {
        if (m_isGrabbable == false)
            return;

        base.OnClickHeld(vrInteraction);

        Vector2 inputAxis = vrInteraction.m_laser.GetThumbAxisValue;

        if (vrInteraction.m_laser.GetOtherLaser.GetTriggerEvents.inputPressed.Value)
        {
            transform.Rotate(Camera.main.transform.right, inputAxis.y, Space.World);
            transform.Rotate(Camera.main.transform.up, -inputAxis.x, Space.World);
        }
        else if (Vector3.Distance(vrInteraction.m_handTrans.position, transform.position) < vrInteraction.m_laser.GetMaxLaserDistance || inputAxis.y < 0)
            transform.position = transform.position + vrInteraction.m_handTrans.forward * reelInSpeed * inputAxis.y;
    }


    private void FixedUpdate()
    {
        if (m_rigidbody)
        {
            if (m_grabed)
            {
                velo *= Time.fixedDeltaTime;

                velo += (transform.position - oldPos) * Time.fixedDeltaTime;


                oldPos = transform.position;
            }

            if (transform.position.y < killY)
            {
                transform.localScale = startScale;
                transform.rotation = startRot;
                m_rigidbody.velocity = Vector3.zero;
                transform.position = startPos;
            }
        }
    }


    override public void OnSecondaryClick(VRInteractionData vrInteractionData)
    {
        if (m_isGrabbable == false)
            return;

        base.OnSecondaryClick(vrInteractionData);

        StartHold(vrInteractionData);
    }

    override public void OnSecondaryClickRelease(VRInteractionData vrInteractionData)
    {
        if (m_isGrabbable == false)
            return;

        base.OnSecondaryClickRelease(vrInteractionData);


        EndHold(vrInteractionData);
    }


    public override void OnSecondaryClickHeld(VRInteractionData vrInteraction)
    {
        if (m_isGrabbable == false)
            return;

        Vector2 inputAxis = vrInteraction.m_laser.GetThumbAxisValue;

        if (vrInteraction.m_laser.GetTriggerEvents.inputPressed.Value == false)
        {
            if (vrInteraction.m_laser.GetOtherLaser.GetTriggerEvents.inputPressed.Value)
            {
                transform.Rotate(Camera.main.transform.right, inputAxis.y, Space.World);
                transform.Rotate(Camera.main.transform.up, -inputAxis.x, Space.World);
            }
            else if (Vector3.Distance(vrInteraction.m_handTrans.position, transform.position) < vrInteraction.m_laser.GetMaxLaserDistance || inputAxis.y < 0)
                transform.position = transform.position + vrInteraction.m_handTrans.forward * reelInSpeed * inputAxis.y;
        }

        base.OnSecondaryClickHeld(vrInteraction);


    }

    public override void OnHoverEnter(VRInteractionData vrInteraction)
    {
        hoverCounts++;

        if (hoverCounts == 1)
        {
            base.OnHoverEnter(vrInteraction);

            if (m_highlightOn)
                m_outliner.enabled = true;
        }
    }

    public override void OnHoverExit(VRInteractionData vrInteraction)
    {
        hoverCounts--;

        if (hoverCounts == 0)
        {
            base.OnHoverExit(vrInteraction);

            if (m_highlightOn && m_grabed == false)
                m_outliner.enabled = false;
        }
    }

    public override WhatIsHold GetWhatIsHold()
    {
        return WhatIsHold.Secondary;
    }


    public void GrabObject(Laser laser)
    {
        OnSecondaryClick(laser.m_vrInteractionData);
    }

    public void GrabHoldObject(Laser laser)
    {
        OnSecondaryClickHeld(laser.m_vrInteractionData);
    }

    public void GrabHoldRelease(Laser laser)
    {
        OnSecondaryClickRelease(laser.m_vrInteractionData);
    }

    public void UpdateScale(Transform laser, Transform otherLaser)
    {
        float currentScaleDistance = Vector3.Distance(laser.position, otherLaser.position);
        if (m_clampScale)
        {
            Vector3 newScale = transform.localScale + Vector3.one * (currentScaleDistance - m_laserScaleStartDistance) * scaleSpeed;

            if ((newScale.x > m_maxScale || newScale.y > m_maxScale || newScale.z > m_maxScale || newScale.x < m_minScale || newScale.y < m_minScale || newScale.z < m_minScale) == false)
                transform.localScale = newScale;



        }
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

    protected void StartHold(VRInteractionData vrInteractionData)
    {
        if (m_grabed == true)
            return;

        if (m_rigidbody)
            m_rigidbody.isKinematic = true;
        
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

            if (m_highlightOn && hoverCounts == 0)
                m_outliner.enabled = false;

            if (m_rigidbody)
            {
                if (m_keepKinematic == false)
                    m_rigidbody.isKinematic = false;
                m_rigidbody.AddForce(velo * 2000, ForceMode.VelocityChange);
            }

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
