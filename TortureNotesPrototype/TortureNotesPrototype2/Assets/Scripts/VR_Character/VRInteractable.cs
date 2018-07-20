using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractable : MonoBehaviour
{
    public virtual void OnHoverEnter(VRInteractionData vrInteraction)
    {

    }
    public virtual void OnHoverExit(VRInteractionData vrInteraction)
    {

    }
    public virtual void OnClick(VRInteractionData vrInteraction)
    {

    }
    public virtual void OnClickHeld(VRInteractionData vrInteraction)
    {

    }
    public virtual void OnClickRelease(VRInteractionData vrInteraction)
    {

    }
}

public struct VRInteractionData
{
    public Vector3 pos;
    public Transform handTrans;
    public Vector2 movementDirection;
    public bool secondaryClickPressed;
    public System.Action<Color> changeColor;
}