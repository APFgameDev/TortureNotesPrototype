using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOnHit : MonoBehaviour
{
    public KeyboardKey m_ParentKey;

    private void OnTriggerEnter(Collider other)
    {
        if (m_ParentKey != null)
        {
            if (other.tag == "Keyboard Mallet")
            {
                m_ParentKey.OnHitEnter(other);
            }
        }
        else
        {
            Debug.LogError("Parent key not set!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_ParentKey != null)
        {
            if (other.tag == "Keyboard Mallet")
            {
                m_ParentKey.OnHitExit(other);
            }
        }
        else
        {
            Debug.LogError("Parent key not set!");
        }
    }
}
