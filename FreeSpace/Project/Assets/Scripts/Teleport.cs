using Annotation.SO.UnityEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Teleport : MonoBehaviour
{
    [SerializeField]
    UnityEventVoidSO m_turnOnTeleport;

    [SerializeField]
    UnityEventVoidSO m_Teleport;

    LineRenderer m_lineRenderer;

    [SerializeField]
    float velocity;

    [SerializeField]
    float Gravity = 9;

    [SerializeField]
    float m_height = 1;

    [SerializeField]
    Transform user;

    [SerializeField]
    ushort m_resolution;

    Vector3 telePosition;
    bool hasPlaceToTeleport = false;

    private void Start()
    {
        m_turnOnTeleport.UnityEvent.AddListener(StartTeleportationSequence);
        m_Teleport.UnityEvent.AddListener(DoTeleport);

        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
        m_lineRenderer.positionCount = m_resolution;
    }

    void StartTeleportationSequence()
    {
        StartCoroutine(TelportationSequence());
    }

    void DoTeleport()
    {
        StopAllCoroutines();

        m_lineRenderer.enabled = false;

        if (hasPlaceToTeleport)
            user.position = telePosition + Vector3.up * m_height;
    }

    IEnumerator TelportationSequence()
    {

        Vector3[] positions = new Vector3[m_resolution];
        m_lineRenderer.enabled = true;

        while (true)
        {
            hasPlaceToTeleport = false;
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    float t = (float)i / (float)m_resolution * 2;

                    positions[i] = transform.position + CalculateArcPoint(t, transform.forward);
                }



                m_lineRenderer.material.SetColor("_Color", Color.grey);
            }
            {
                int i = 0;
                for (; i < positions.Length - 1; i++)
                {
                    RaycastHit raycastHit;

                    Vector3 direction = positions[i + 1] - positions[i];
                    if (Physics.Raycast(new Ray(positions[i], direction), out raycastHit, direction.magnitude, ~LayerMask.NameToLayer(Layers.Ground)))
                    {
                        if (Vector3.Dot(raycastHit.normal, Vector3.up) > 0.5f)
                        {
                            hasPlaceToTeleport = true;
                            m_lineRenderer.material.SetColor("_Color", Color.green);
                            positions[i] = telePosition = raycastHit.point;
                            break;
                        }
                    }
                }

                m_lineRenderer.positionCount = i + 1;

    

                m_lineRenderer.SetPositions(positions);
            }
            yield return null;
        }
    }

    Vector3 CalculateArcPoint(float t,Vector3 Direction)
    {
        float y = -Gravity * t * t;

        return Direction * t * velocity + new Vector3(0,y,0);
    }
}
