using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardKeySetup : MonoBehaviour
{
    public GameObject[] m_Keys;

    [Range(0.0f, 1.0f)]
    public float m_KeyHorizontalSpacing;

    public void SetKeys()
    {
        Vector3 previousPosition = m_Keys[0].transform.localPosition;

        int halfIndex = (m_Keys.Length / 2);

        List<GameObject> LeftList = new List<GameObject>();
        List<GameObject> RightList = new List<GameObject>();

        for (int i = halfIndex + 1; i < m_Keys.Length; i++)
        {
            RightList.Add(m_Keys[i]);
        }

        for (int i = halfIndex - 1; i >= 0; i--)
        {
            LeftList.Add(m_Keys[i]);
        }

        Vector3 prevPos = m_Keys[halfIndex].transform.localPosition;
        //Add offset to right side
        for (int i = 0; i < RightList.Count; i++)
        {
            Vector3 newPos = prevPos;
            newPos.x += m_KeyHorizontalSpacing;
            RightList[i].transform.localPosition = newPos;

            prevPos = RightList[i].transform.localPosition;
        }

        //prevPos = m_Keys[halfIndex].transform.localPosition;
        ////Add offset to left side
        //for (int i = LeftList.Count - 1; i >= 0; i--)
        //{
        //    Vector3 newPos = prevPos;
        //    newPos.x -= m_KeyHorizontalSpacing;
        //    LeftList[i].transform.localPosition = newPos;
        //
        //    prevPos = LeftList[i].transform.localPosition;
        //}

        prevPos = m_Keys[halfIndex].transform.localPosition;
        //Add offset to left side
        for (int i = 0; i < LeftList.Count; i++)
        {
            Vector3 newPos = prevPos;
            newPos.x -= m_KeyHorizontalSpacing;
            LeftList[i].transform.localPosition = newPos;

            prevPos = LeftList[i].transform.localPosition;
        }
    }
}
