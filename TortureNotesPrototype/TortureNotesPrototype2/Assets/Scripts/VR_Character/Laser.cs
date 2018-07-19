using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField]
    bool IsOn = true;

    [SerializeField]
    InputAxisIndex inputAxisIndex;

    [SerializeField]
    float valueTest;

    LineRenderer lineRenderer;
    VRSelectableHandler m_selectableHandler;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }
    void Update()
    {
        if (IsOn)
        {
            Vector3 StartPos = transform.position;
            Vector3 ForwardPos = transform.forward * 20;
            lineRenderer.SetPosition(0, StartPos);
            lineRenderer.SetPosition(1, ForwardPos);

            RaycastHit hit;
            if (Physics.Raycast(StartPos, ForwardPos - StartPos, out hit))
            {
                if (m_selectableHandler && Input.GetAxis(InputAxis.InputAxisArray[(int)inputAxisIndex]) > 0.5f)
                    m_selectableHandler.Select(hit.point);

                GameObject other = hit.collider.gameObject;
                VRSelectableHandler selectableHandler = other.GetComponent<VRSelectableHandler>();
                if (selectableHandler != m_selectableHandler)
                {
                    if (m_selectableHandler)
                        m_selectableHandler.DecrementSelection();

                    m_selectableHandler = selectableHandler;

                    if (m_selectableHandler)
                        m_selectableHandler.IncrementSelection();
                }                

                ////Just testing this for now
                //if (other != null)
                //{
                //    if (other.tag.ToLower() == "selectable")
                //    {
                //        if (Input.GetAxis(InputAxis.RightGripTrigger) > 0.5f)
                //        {
                //            other.gameObject.SetActive(false);
                //        }
                //    }
                //}
            }
            else if (m_selectableHandler)
            {
                m_selectableHandler.DecrementSelection();
                m_selectableHandler = null;
            }


        }
    }
}
