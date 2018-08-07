using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnnotatable : VRInteractable
{
    [SerializeField]
    private GameObject m_annotationPrefab;
    [SerializeField]
    private float m_PlacementOffset;
    private bool m_isDrawing;

    [SerializeField]
    private Annotation.SO.BoolReference m_IsTriggerHeld;

    private Transform m_CurrentInteractionHand = null;

    private float m_CalculatedPlacementOffset;
    private Vector3 m_highlightPlacePoint;

    private void Start()
    {
        //Attempt to get the mesh filter to calculate proper offset based on the mesh bounds and the placement offset value
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if(meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            m_CalculatedPlacementOffset = mesh.bounds.extents.magnitude + m_PlacementOffset;
        }

        StickyHover = false;
    }

    public void StartDrawing(VRInteractionData vrInteraction)
    {           
        if(m_isDrawing == false)
        {
            m_isDrawing = true;
            m_CurrentInteractionHand = vrInteraction.handTrans;
            m_highlightPlacePoint = m_hitPoint;
        }
    }

    public void PlaceAnnotation(VRInteractionData vrInteraction)
    {
        Vector3 randomPosition = Random.insideUnitCircle * m_CalculatedPlacementOffset;
        randomPosition.y = Mathf.Abs(randomPosition.y);
        randomPosition = Camera.main.transform.rotation * randomPosition + transform.position;
        
        VRAnnotation annotation = Instantiate(m_annotationPrefab, randomPosition, Quaternion.identity, transform).GetComponentInChildren<VRAnnotation>();
        annotation.StartUp(m_highlightPlacePoint);

        OnFinish();
    }

    public void StartDrag(VRInteractionData vrInteraction)
    {     
        if (vrInteraction.handTrans == m_CurrentInteractionHand && m_isDrawing)
        {
            //get VRAnnotation componet call setup
            VRAnnotation vRAnnotation = Instantiate(m_annotationPrefab).GetComponentInChildren<VRAnnotation>();
            vrInteraction.handTrans.GetComponent<Annotation.Laser>().ForceHoldObject(vRAnnotation, false);
            vRAnnotation.transform.position = vrInteraction.GetClosestLaserPoint(m_highlightPlacePoint);
            vRAnnotation.StartUp(m_highlightPlacePoint);
            OnFinish();
        }
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
}
