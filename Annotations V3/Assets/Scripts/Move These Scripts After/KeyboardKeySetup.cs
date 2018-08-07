using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardKeySetup : MonoBehaviour
{
    public GameObject[] m_Keys;

    [Range(0.10f, 0.30f)]
    public float m_KeyHorizontalSpacing;

    public void SetKeys()
    {
        Vector3 previousPosition = m_Keys[0].transform.localPosition;

        //Start at 1, dont want to space the first key
        for (int i = 1; i < m_Keys.Length; i++)
        {
            Vector3 newPos = previousPosition;
            newPos.x += m_KeyHorizontalSpacing;
            m_Keys[i].transform.localPosition = newPos;

            previousPosition = m_Keys[i].transform.localPosition;
        }
    }
}
