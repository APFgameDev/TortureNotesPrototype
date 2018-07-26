using UnityEngine.Events;
using UnityEngine;

public class VRInteractable : MonoBehaviour
{
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
    public System.Func<Vector3, Vector3> GetClosestLaserPoint;
    public Transform handTrans;
    public Vector2 movementDirection;
    public bool secondaryClickPressed;
    public System.Action<Color> changeColor;
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
