using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class Keyboard : MonoBehaviour
{
    public KeyboardSO m_KeyboardSO;

    private void Awake()
    {
        m_KeyboardSO.m_OnTurnOn.AddListener(delegate { SetActive(true); });
        m_KeyboardSO.m_OnTurnOff.AddListener(delegate { SetActive(false); });
    }

    /// <summary>
    /// Used to turn the keyboard on or off
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
