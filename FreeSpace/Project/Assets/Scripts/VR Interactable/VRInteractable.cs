using UnityEngine.Events;
using UnityEngine;
using Annotation;

[System.Serializable]
public class VRInteractable : MonoBehaviour
{
    [SerializeField]
    bool isInteractable = true;

    public bool m_interactable { get { return isInteractable; } set { isInteractable = value; } }

    protected bool m_stickyHover = true;
    public bool StickyHover { get { return m_stickyHover; } protected set { m_stickyHover = value; } }

    [SerializeField]
    private InteractEvents m_Events = new InteractEvents();

    public virtual void OnHoverEnter(VRInteractionData vrInteraction)
    {
        m_Events.OnHoverEnter.Invoke(vrInteraction);
    }
    public virtual void OnHoverExit(VRInteractionData vrInteraction)
    {
        m_Events.OnHoverExit.Invoke(vrInteraction);
    }
    public virtual void OnClick(VRInteractionData vrInteraction)
    {
        m_Events.OnClick.Invoke(vrInteraction);
    }
    public virtual void OnClickHeld(VRInteractionData vrInteraction)
    {
        m_Events.OnClickHeld.Invoke(vrInteraction);
    }
    public virtual void OnClickRelease(VRInteractionData vrInteraction)
    {
        m_Events.OnClickRelease.Invoke(vrInteraction);
    }
    public virtual void OnSecondaryClick(VRInteractionData vrInteraction)
    {
        m_Events.OnSecondaryClick.Invoke(vrInteraction);
    }
    public virtual void OnSecondaryClickHeld(VRInteractionData vrInteraction)
    {
        m_Events.OnSecondaryClickHeld.Invoke(vrInteraction);
    }
    public virtual void OnSecondaryClickRelease(VRInteractionData vrInteraction)
    {
        m_Events.OnSecondaryClickRelease.Invoke(vrInteraction);
    }
    public virtual WhatIsHold GetWhatIsHold()
    {
        return WhatIsHold.None;
    }
}

[System.Serializable]
public struct VRInteractionData
{
    public System.Func<Vector3, Vector3, Vector3> GetClosestLaserPointOnPlane;
    public System.Func<Vector3, Vector3> GetClosestLaserPoint;
    public Transform m_handTrans;
    public Laser m_laser;
    public Vector3 m_hitPoint;
    public System.Action<Color> ChangeColor;
}

[System.Serializable]
public class InteractEvents
{
    public VRInteractionEvent OnHoverEnter = new VRInteractionEvent();
    public VRInteractionEvent OnHoverExit = new VRInteractionEvent();
    public VRInteractionEvent OnClick = new VRInteractionEvent();
    public VRInteractionEvent OnClickHeld = new VRInteractionEvent();
    public VRInteractionEvent OnClickRelease = new VRInteractionEvent();
    public VRInteractionEvent OnSecondaryClick = new VRInteractionEvent();
    public VRInteractionEvent OnSecondaryClickHeld = new VRInteractionEvent();
    public VRInteractionEvent OnSecondaryClickRelease = new VRInteractionEvent();
}

[System.Serializable]
public class VRInteractionEvent : UnityEvent<VRInteractionData>
{
}

public enum WhatIsHold
{
    Primary,
    Secondary,
    None
}
