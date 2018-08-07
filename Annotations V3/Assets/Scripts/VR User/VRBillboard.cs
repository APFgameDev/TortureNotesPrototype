using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBillboard : MonoBehaviour
{
    private Transform m_Camera;

	void Start ()
    {
        m_Camera = Camera.main.transform;
	}
	
	void Update ()
    {
        transform.LookAt(m_Camera);
	}
}
