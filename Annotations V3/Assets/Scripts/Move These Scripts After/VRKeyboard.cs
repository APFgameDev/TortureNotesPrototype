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
    public GameObject[] m_AlphabetKeyParentObjects;
    public GameObject[] m_PunctuationKeyParentObjects;

    [Header("Panels for keyboard display")]
    [SerializeField] private GameObject m_TextPanel;
    [SerializeField] private GameObject m_OptionsPanel;

    [Header("Publish these events when when you Open/Close the options menu")]
    [SerializeField] private VoidEvent m_OptionsMenuOpenEvent;
    [SerializeField] private VoidEvent m_OptionsMenuCloseEvent;

    private void Awake()
    {
        m_KeyboardSO.InvokeTurnOn();
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
        m_KeyboardSO.m_DoneTypingEvent.Publish();

        InvokeTurnOffInKeyboardSO();
    }

    public void SwitchKeys()
    {
        foreach (GameObject obj in m_AlphabetKeyParentObjects)
        {
            obj.SetActive(!obj.activeInHierarchy);
        }

        foreach (GameObject obj in m_PunctuationKeyParentObjects)
        {
            obj.SetActive(!obj.activeInHierarchy);
        }
    }

    public void CancelTyping()
    {
        m_KeyboardSO.InvokeTurnOff();
    }

    public void ClearString()
    {
        m_KeyboardSO.m_KeyboardInputSO.Value = string.Empty;
    }

    public void InvokeTurnOnInKeyboardSO()
    {
        m_KeyboardSO.InvokeTurnOn();
    }

    public void InvokeTurnOffInKeyboardSO()
    {
        m_KeyboardSO.InvokeTurnOff();
    }

    public void ToggleKeyboardCanvas()
    {
        m_TextPanel.SetActive(!m_TextPanel.activeInHierarchy);

        m_OptionsPanel.SetActive(!m_TextPanel.activeInHierarchy);
    }
}
