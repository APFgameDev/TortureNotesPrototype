using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class KeyOnHover : MonoBehaviour
{
    [Header("Key associated with this OnHover Script")]
    [SerializeField] private KeyboardKey m_Key;

    private void Awake()
    {
        if (m_Key == null)
        {
            m_Key = GetComponentInParent<KeyboardKey>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Keyboard Mallet")
        {
            m_Key.OnHoverEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Keyboard Mallet")
        {
            m_Key.OnHoverExit();
        }
    }
}
