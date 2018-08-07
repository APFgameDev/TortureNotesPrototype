using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnnotatable : VRInteractable
{
    [SerializeField]
    private GameObject m_annotationPrefab;

    private bool m_isDrawing;

    public override void OnClick(VRInteractionData vrInteraction)
    {
        base.OnClick(vrInteraction);

        if(m_isDrawing == false)
        {
            m_isDrawing = true;
            // get VRAnnotation componet call setup
            Instantiate(m_annotationPrefab);
            
        }
    }

    public void RemoveAll()
    {
        // Get VRAnnotation Components remove / delete them
    }

    void OnFinish()
    {
        m_isDrawing = false;
    }

}
