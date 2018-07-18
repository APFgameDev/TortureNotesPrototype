using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool IsOn = true;
    public LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }
    void Update()
    {
        if(IsOn)
        {
            Vector3 StartPos = transform.position;
            Vector3 ForwardPos = transform.forward * 20;
            lineRenderer.SetPosition(0, StartPos);
            lineRenderer.SetPosition(1, ForwardPos);

            RaycastHit hit;
            if (Physics.Raycast(StartPos, ForwardPos - StartPos, out hit))
            {
                GameObject other = hit.collider.gameObject;

                //Just testing this for now
                if(other != null)
                {
                    if (other.tag.ToLower() == "selectable")
                    {
                        if (Input.GetAxis(InputAxis.RightGripTrigger) > 0.5f)
                        {
                            other.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
