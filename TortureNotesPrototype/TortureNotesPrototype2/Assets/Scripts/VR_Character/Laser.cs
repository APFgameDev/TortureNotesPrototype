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
    VRInteractable m_vrInteractable;
    VRInteractionData vrInteraction;

    public static readonly Color DEFAULTLASERCOLOR = new Color(1, 0, 0);

    bool isClicked = false;

    private void Awake()
    {
        vrInteraction.handTrans = transform;
        vrInteraction.changeColor = ChangeLaseColor;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        ChangeLaseColor(DEFAULTLASERCOLOR);
    }
    void Update()
    {
        if (IsOn)
        {
            Vector3 StartPos = transform.position;
            Vector3 ForwardPos = transform.forward * 20;
            lineRenderer.SetPosition(0, StartPos);
            lineRenderer.SetPosition(1, ForwardPos);

            bool isClickHeld = Input.GetAxis(InputAxis.InputAxisArray[(int)clickAxis]) > 0.5f;
            vrInteraction.secondaryClickPressed = Input.GetAxis(InputAxis.InputAxisArray[(int)secondaryClickAxis]) > 0.5f;

            Vector2 movementDirection = new Vector2(
                Input.GetAxis(InputAxis.InputAxisArray[(int)horizontalAxis]),
                Input.GetAxis(InputAxis.InputAxisArray[(int)verticalAxis])
                ).normalized;

            vrInteraction.pos = CalculateClosestPointOnLaserFromInteractable();
            moveDirTest = vrInteraction.movementDirection = movementDirection;

            if (m_vrInteractable)
            { 
                if (isClickHeld)
                    m_vrInteractable.OnClickHeld(vrInteraction);
                else
                    m_vrInteractable.OnClickRelease(vrInteraction);
            }

            RaycastHit hit;
            if (Physics.Raycast(StartPos, ForwardPos - StartPos, out hit))
            {
                if (isClickHeld == true && isClicked == false)
                {
                    isClicked = true;

                    if (m_vrInteractable)
                        m_vrInteractable.OnClick(vrInteraction);
                }
                else if(isClickHeld == false && isClicked == true)
                    isClicked = false;

                GameObject other = hit.collider.gameObject;

                VRInteractable selectableHandler = other.GetComponent<VRInteractable>();

                if (selectableHandler != m_vrInteractable && isClickHeld == false)
                {
                    if (m_vrInteractable)
                        m_vrInteractable.OnHoverExit(vrInteraction);

                    m_vrInteractable = selectableHandler;

                    if (m_vrInteractable)
                        m_vrInteractable.OnHoverEnter(vrInteraction);
                }                
            }
            else if (m_vrInteractable && isClickHeld == false)
            {
                m_vrInteractable.OnHoverExit(vrInteraction);
                m_vrInteractable = null;
            }
        }
    }

    Vector3 CalculateClosestPointOnLaserFromInteractable()
    {
        if (m_vrInteractable)
            return Vector3.Project((m_vrInteractable.transform.position - transform.position), transform.forward);
        else
            return Vector3.zero;
    }

    void ChangeLaseColor(Color color)
    {
        lineRenderer.material.SetColor("_Color", color);
    }
}
