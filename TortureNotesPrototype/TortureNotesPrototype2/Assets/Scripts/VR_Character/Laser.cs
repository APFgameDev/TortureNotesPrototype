using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField]
    bool IsOn = true;

    [SerializeField]
    InputAxisIndex clickAxis;
    [SerializeField]
    InputAxisIndex secondaryClickAxis;
    [SerializeField]
    InputAxisIndex horizontalAxis;
    [SerializeField]
    InputAxisIndex verticalAxis;

    [SerializeField]
    Vector2 moveDirTest;

    LineRenderer lineRenderer;
    List<VRInteractable> m_vrInteractables = new List<VRInteractable>();
    VRInteractable m_heldInteractable;
    VRInteractionData vrInteraction;

    List<VRInteractable> interactablesCollidedWithThisFrame = new List<VRInteractable>();

    public static readonly Color DEFAULTLASERCOLOR = new Color(1, 0, 0);

    bool isClicked = false;

    private void Awake()
    {
        vrInteraction.handTrans = transform;
        vrInteraction.changeColor = ChangeLaseColor;
        vrInteraction.GetClosestLaserPoint = CalculateClosestPointOnLaserFromInteractable;
        vrInteraction.GetClosestLaserPointOnPlane = CalculateClosestPointOnLaserFromInteractableOnPlane;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        ChangeLaseColor(DEFAULTLASERCOLOR);
    }
    void Update()
    {
        if (IsOn)
        {
            Vector3 StartPos = transform.position;
            Vector3 ForwardPos = transform.position + transform.forward * 20;
            lineRenderer.SetPosition(0, StartPos);
            lineRenderer.SetPosition(1, ForwardPos);

            bool isClickHeld = Input.GetAxis(InputAxis.InputAxisArray[(int)clickAxis]) > 0.5f;
            vrInteraction.secondaryClickPressed = Input.GetAxis(InputAxis.InputAxisArray[(int)secondaryClickAxis]) > 0.5f;

            Vector2 movementDirection = new Vector2(
                Input.GetAxis(InputAxis.InputAxisArray[(int)horizontalAxis]),
                Input.GetAxis(InputAxis.InputAxisArray[(int)verticalAxis])
                ).normalized;


            moveDirTest = vrInteraction.movementDirection = movementDirection;

            interactablesCollidedWithThisFrame.Clear();
            RaycastHit[] hit;
            hit = Physics.RaycastAll(StartPos, ForwardPos - StartPos);

            VRInteractable closestInteractable = null;
            float closestInteractableDist = float.MaxValue;


            for (int i = 0; i < hit.Length; i++)
            {
                GameObject other = hit[i].collider.gameObject;
                VRInteractable vrInteractable = other.GetComponent<VRInteractable>();              
                if (vrInteractable != null && vrInteractable.enabled)
                {
                    vrInteractable.hitPoint = hit[i].point;
                    if (hit[i].distance < closestInteractableDist)
                    {
                        closestInteractable = vrInteractable;
                        closestInteractableDist = hit[i].distance;
                    }
                    interactablesCollidedWithThisFrame.Add(vrInteractable);

                    if (m_vrInteractables.Contains(vrInteractable) == false)
                        vrInteractable.OnHoverEnter(vrInteraction);
                }
            }

            if (isClickHeld && m_heldInteractable == null && closestInteractable != null)
            {
                m_heldInteractable = closestInteractable;
              
            }
            else if (isClickHeld == false && m_heldInteractable != null)
            {
            
                m_heldInteractable.OnClickRelease(vrInteraction);
                m_heldInteractable = null;
            }

            if (m_heldInteractable != null && isClickHeld)
            {
                m_heldInteractable.OnClickHeld(vrInteraction);
            }


            if (isClickHeld == true && isClicked == false)
            {
                isClicked = true;
                if (closestInteractable != null)
                {
                    closestInteractable.OnClick(vrInteraction);
                }
            }
            else if (isClickHeld == false && isClicked == true)
            {
                isClicked = false;
            }


            for (int i = 0; i < m_vrInteractables.Count; i++)
            {
                if (interactablesCollidedWithThisFrame.Contains(m_vrInteractables[i]) == false)
                    m_vrInteractables[i].OnHoverExit(vrInteraction);
            }
            m_vrInteractables.Clear();
            m_vrInteractables.AddRange(interactablesCollidedWithThisFrame);
        }
    }

    Vector3 CalculateClosestPointOnLaserFromInteractable(Vector3 pos)
    {
        return transform.position + Vector3.Project(pos - transform.position, transform.forward);
    }

    Vector3 CalculateClosestPointOnLaserFromInteractableOnPlane(Vector3 pos,Vector3 normal)
    {
       return pos + Vector3.ProjectOnPlane(CalculateClosestPointOnLaserFromInteractable(pos) - pos, normal);
    }

    void ChangeLaseColor(Color color)
    {
        lineRenderer.material.SetColor("_Color", color);
    }
}
