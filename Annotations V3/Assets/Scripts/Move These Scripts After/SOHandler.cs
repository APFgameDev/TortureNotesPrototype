using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class SOHandler : MonoBehaviour
{
    [SerializeField] private BoolVariable m_LaserToggle;

    public void Toggle()
    {
        m_LaserToggle.Value = !m_LaserToggle.Value;
    }
}
