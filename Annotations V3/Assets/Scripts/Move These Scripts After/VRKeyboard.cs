using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class VRKeyboard : Keyboard
{
    [Header("Main Keyboard")]
    [SerializeField]
    private KeyboardSO m_KeyboardSO;

    [Header("Keys for switching state")]
    public GameObject m_AlphabetKeys;
    public GameObject m_PunctuationKeys;

    [SerializeField]
    private GameObject[] m_TurnOffKeyboardObjects;
    [SerializeField]
    private GameObject[] m_TurnOnKeyboardObjects;

    private void Awake()
    {
        m_KeyboardSO.m_OnTurnOn.AddListener(delegate { ToggleKeyboard(false); });
        m_KeyboardSO.m_OnTurnOff.AddListener(delegate { ToggleKeyboard(true); });

        SetActive(false);
    }

    public void ToggleCaps()
    {
        m_KeyboardSO.m_IsCaps.Value = !m_KeyboardSO.m_IsCaps.Value;
    }

    public void RemoveString()
    {
        m_KeyboardSO.RemoveString();
    }

    public void DoneTyping()
    {
        m_KeyboardSO.m_OnPublish.Invoke();        
    }

    public void SwitchKeys()
    {
        m_AlphabetKeys.SetActive(!m_AlphabetKeys.activeInHierarchy);
        m_PunctuationKeys.SetActive(!m_PunctuationKeys.activeInHierarchy);
    }
    
    public void ToggleKeyboard(bool toggle)
    {
        foreach (GameObject obj in m_TurnOffKeyboardObjects)
        {
            if (obj != null)
                obj.SetActive(toggle);
        }

        foreach (GameObject obj in m_TurnOnKeyboardObjects)
        {
            if (obj != null)
                obj.SetActive(!toggle);
        }

        SetActive(!toggle);
    }
}
