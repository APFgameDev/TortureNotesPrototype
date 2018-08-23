using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mallet : MonoBehaviour
{
    Vector3 oldPos;
    KeyOnHit m_keyOnHit;
    KeyOnHover m_keyOnHover;
    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector3 direction = transform.position - oldPos;

        RaycastHit raycastHit;
        KeyOnHit keyOnHit = null;
        KeyOnHover keyOnHover = null;

        if (Physics.SphereCast(oldPos,0.5f * transform.lossyScale.x, direction, out raycastHit, direction.magnitude,~ LayerMask.NameToLayer(Layers.Key)))
        {
            keyOnHit = raycastHit.transform.GetComponent<KeyOnHit>();
            keyOnHover = raycastHit.transform.GetComponent<KeyOnHover>();
        }


        if (m_keyOnHit != keyOnHit)
        {
            if(m_keyOnHit != null)
                m_keyOnHit.CallKeyExit();
            if (keyOnHit != null)
                keyOnHit.CallKeyHit(-direction);
        }

        m_keyOnHit = keyOnHit;

        if (m_keyOnHover != keyOnHover)
        {
            if (m_keyOnHover != null)
                m_keyOnHover.CallKeyHitExit();
            if (keyOnHover != null)
                keyOnHover.CallKeyHit();
        }

        m_keyOnHover = keyOnHover;


        oldPos = transform.position;


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f * transform.lossyScale.x);
    }
}
