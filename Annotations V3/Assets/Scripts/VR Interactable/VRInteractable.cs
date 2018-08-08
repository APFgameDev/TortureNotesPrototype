using UnityEngine.Events;
using UnityEngine;
using Annotation;

public class VRInteractable : MonoBehaviour
{
    public Vector3 m_hitPoint;

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
}

[System.Serializable]
public struct VRInteractionData
{
    public System.Func<Vector3, Vector3, Vector3> GetClosestLaserPointOnPlane;
    public System.Func<Vector3, Vector3> GetClosestLaserPoint;
    public Transform m_handTrans;
    public Laser m_laser;
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
}

[System.Serializable]
public class VRInteractionEvent : UnityEvent<VRInteractionData>
{
}
