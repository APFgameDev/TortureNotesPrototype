using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnnotatable : VRGrabbable
{
    [SerializeField]
    private GameObject m_annotationPrefab;
    [SerializeField]
    private float m_PlacementOffset;
    private bool m_isDrawing;
    [SerializeField]
    Annotation.SO.BoolVariable m_isEditing;

    [SerializeField]
    Transform parent;


    private Transform m_CurrentInteractionHand = null;

    private float m_CalculatedPlacementOffset;
    private Vector3 m_highlightPlacePoint;
    private Mesh mesh;

    private void Start()
    {
        //Attempt to get the mesh filter to calculate proper offset based on the mesh bounds and the placement offset value
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (parent == null)
            parent = transform;

        if(meshFilter != null)
        {
            mesh = meshFilter.mesh;
            m_CalculatedPlacementOffset = mesh.bounds.extents.magnitude + m_PlacementOffset;
        }


        m_outliner.enabled = false;
        StickyHover = false;
    }

    public override void OnHoverExit(VRInteractionData vrInteraction)
    {
        base.OnHoverExit(vrInteraction);

        if (vrInteraction.m_handTrans == m_CurrentInteractionHand && m_isDrawing)
        {
            //get VRAnnotation componet call setup
            VRAnnotation vRAnnotation = Instantiate(m_annotationPrefab).GetComponentInChildren<VRAnnotation>();
            vrInteraction.m_laser.ForceHoldObject(vRAnnotation, false);
            vRAnnotation.transform.position = vrInteraction.GetClosestLaserPoint(m_highlightPlacePoint);
            vRAnnotation.StartUp(m_highlightPlacePoint, parent, true);
            OnFinish();
        }
    }

    public override void OnClick(VRInteractionData vrInteraction)
    {
        base.OnClickRelease(vrInteraction);

        if (m_isDrawing == false && m_isEditing.Value == false && m_grabed == false)
        {
            m_isDrawing = true;
            m_CurrentInteractionHand = vrInteraction.m_handTrans;
            m_highlightPlacePoint = vrInteraction.m_hitPoint;
        }

    
    }

    public override void OnClickRelease(VRInteractionData vrInteraction)
    {
        base.OnClickRelease(vrInteraction);

        if (m_isEditing.Value == true || m_grabed == true)
            return;

        Vector3 randomPosition = Vector3.zero;
        randomPosition.x = Random.Range(-1f,1f) * m_CalculatedPlacementOffset * transform.lossyScale.magnitude;
        randomPosition.y = Random.Range(1f, 3f) + m_CalculatedPlacementOffset * transform.lossyScale.magnitude;
        randomPosition = Camera.main.transform.rotation * randomPosition + transform.position;

        VRAnnotation annotation = Instantiate(m_annotationPrefab, randomPosition, Quaternion.identity, null).GetComponentInChildren<VRAnnotation>();
        annotation.StartUp(m_highlightPlacePoint, transform, false);

        annotation.GetComponent<VRBillboard>().enabled = true;
        OnFinish();
    }

    public void RemoveAll()
    {
        VRAnnotation[] annotations = GetComponentsInChildren<VRAnnotation>(true);

        for(int i = 0; i < annotations.Length; i++)
        {
            Destroy(annotations[i].gameObject);
        }
    }

    void OnFinish()
    {
        m_isDrawing = false;
        m_CurrentInteractionHand = null;
    }

    private void OnDestroy()
    {
        m_isEditing.Value = false;
    }
}
