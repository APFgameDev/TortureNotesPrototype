using UnityEngine.Events;
using UnityEngine;

public class VRInteractable : MonoBehaviour
{
    [SerializeField]
    private InteractEvents m_Events;

    public virtual void OnHoverEnter(VRInteractionData vrInteraction)
    {
        m_Events.OnHoverEnter.Invoke();
    }
    public virtual void OnHoverExit(VRInteractionData vrInteraction)
    {
        m_Events.OnHoverExit.Invoke();
    }
    public virtual void OnClick(VRInteractionData vrInteraction)
    {
        m_Events.OnClick.Invoke();
    }
    public virtual void OnClickHeld(VRInteractionData vrInteraction)
    {
        m_Events.OnClickHeld.Invoke();
    }
    public virtual void OnClickRelease(VRInteractionData vrInteraction)
    {
        m_Events.OnClickRelease.Invoke();
    }
}

public struct VRInteractionData
{
    public System.Func<Vector3,Vector3> GetClosestLaserPoint;
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
