using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class VR_UI_Interactable : VRInteractable
{
    [SerializeField]
    protected RectTransform selectableRect;

    [SerializeField]
    protected Vector3 m_boxColliderOffset = Vector3.zero;

    [SerializeField]
    bool m_useBoxCollider = true;

    private BoxCollider m_boxCollider;

    private const float BOX_COLLIDER_Z_THICKNESS = 0.01f;

    private void Awake()
    {
        InitVR_UI_Interactable();
    }

    protected void InitVR_UI_Interactable()
    {
        if (selectableRect == null)
            selectableRect = GetComponent<RectTransform>();
        if (GetComponent<BoxCollider>() == null && m_useBoxCollider == true)
            m_boxCollider = RectTransformMathUtility.AddTriggerBoxToRectTransform(selectableRect, m_boxColliderOffset, GetRelativeZThickness());
    }

    private void OnRectTransformDimensionsChange()
    {
        if (selectableRect != null && m_boxCollider != null)
            RectTransformMathUtility.SetBoxCollidersizeToRect(selectableRect.rect, m_boxColliderOffset, m_boxCollider, GetRelativeZThickness());
    }

    float GetRelativeZThickness()
    {
        return BOX_COLLIDER_Z_THICKNESS / (transform.localScale.z > 0f ? transform.lossyScale.z : 0);
    }

    public override WhatIsHold GetWhatIsHold()
    {
        return WhatIsHold.Primary;
    }
}
