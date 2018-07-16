using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Annotation : MonoBehaviour
{
    public LineRenderer linrenderer;
    public Text textComp;

    private Camera m_MainCamera;

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(m_MainCamera.transform);
    }
}
