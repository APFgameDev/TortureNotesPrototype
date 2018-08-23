using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;
using Annotation.SO.UnityEvents;

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
    [SerializeField] private UnityEventVoidSO m_OptionsMenuOpenEvent;
    [SerializeField] private UnityEventVoidSO m_OptionsMenuCloseEvent;


    [SerializeField]
    Vector2Variable leftAxis;

    [SerializeField]
    Vector2Variable rightAxis;

    private void Awake()
    {
        m_KeyboardSO.InvokeTurnOn();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCursor());
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
        m_KeyboardSO.m_KeyboardInputSO.Value = m_KeyboardSO.beforeText;
        m_KeyboardSO.AppendString(string.Empty);
        m_KeyboardSO.m_DoneTypingEvent.Publish();
        m_KeyboardSO.InvokeTurnOff();

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

        m_KeyboardSO.m_InOptionsMenu.Value = m_OptionsPanel.activeInHierarchy;

        if (m_OptionsPanel.activeInHierarchy == true)
        {
            m_OptionsMenuOpenEvent.Invoke();
        }
        else
        {
            m_OptionsMenuCloseEvent.Invoke();
        }
    }



    IEnumerator MoveCursor()
    {
        while (true)
        {
            Vector2 dir = leftAxis.Value + rightAxis.Value;

            if (dir.x < -0.1f && m_KeyboardSO.m_insertTextIndex > 0)
            {
                m_KeyboardSO.m_insertTextIndex--;
                yield return new WaitForSeconds(0.2f);
            }
            else if (dir.x > 0.1f && m_KeyboardSO.m_insertTextIndex < m_KeyboardSO.m_KeyboardInputSO.Value.Length)
            {
                m_KeyboardSO.m_insertTextIndex++;
                yield return new WaitForSeconds(0.2f);
            }


            yield return null;
        }
    }
}
