using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class KeyOnHit : MonoBehaviour
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

    public void CallKeyHit(Vector3 direction)
    {
        m_Key.OnHitEnter(direction);
    }

    public void CallKeyExit()
    {
        m_Key.OnHitExit();
    }
}