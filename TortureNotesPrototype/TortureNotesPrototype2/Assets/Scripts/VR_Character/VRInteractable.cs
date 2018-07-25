using UnityEngine.Events;
using UnityEngine;

public class VRInteractable : MonoBehaviour
{
    [SerializeField]
    private InteractEvents m_Events = new InteractEvents();

    public virtual void OnHoverEnter(VRInteractionData vrInteraction)
    {
        if(m_Events.OnHoverEnter != null)
            m_Events.OnHoverEnter.Invoke();
    }
    public virtual void OnHoverExit(VRInteractionData vrInteraction)
    {
        if (m_Events.OnHoverExit != null)
            m_Events.OnHoverExit.Invoke();
    }
    public virtual void OnClick(VRInteractionData vrInteraction)
    {
        if (m_Events.OnClick != null)
            m_Events.OnClick.Invoke();
    }
    public virtual void OnClickHeld(VRInteractionData vrInteraction)
    {
        if (m_Events.OnClickHeld != null)
            m_Events.OnClickHeld.Invoke();
    }
    public virtual void OnClickRelease(VRInteractionData vrInteraction)
    {
        if (m_Events.OnClickRelease != null)
            m_Events.OnClickRelease.Invoke();
    }
}

public struct VRInteractionData
{
    public System.Func<Vector3, Vector3> GetClosestLaserPoint;
    public Transform handTrans;
    public Vector2 movementDirection;
    public bool secondaryClickPressed;
    public System.Action<Color> changeColor;
}

[System.Serializable]
public struct InteractEvents
{
    public UnityEvent OnHoverEnter;
    public UnityEvent OnHoverExit;
    public UnityEvent OnClick;
    public UnityEvent OnClickHeld;
    public UnityEvent OnClickRelease;
}
