using Annotation.SO;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI), typeof(VoidEventListener))]
public class VRText : MonoBehaviour
{
    [SerializeField]
    private KeyboardSO m_KeyboardSO;

    private TextMeshProUGUI m_TextComp;

    private VoidEventListener m_VoidEventListener;


    string m_text = "";

    bool editing = false;



    private void Awake()
    {
        m_VoidEventListener = GetComponent<VoidEventListener>();
    }

    private void OnEnable()
    {
        if (m_KeyboardSO == null)
        {
            Debug.LogError("The keyboard so / input string variable are not set. Please set these.", this);
        }

        if (m_TextComp == null) m_TextComp = GetComponent<TextMeshProUGUI>();
    }

    public void StartListening()
    {
        if (editing == false)
        {
            m_KeyboardSO.m_onAppendString.UnityEvent.AddListener(AppendText);
            m_KeyboardSO.m_DoneTypingEvent.SubscribeListener(m_VoidEventListener);
            editing = true;

            StartCoroutine(EditingText());
        }
    }

    public void AppendText(string text)
    {
        m_text = text;
    }


    public void OnDone()
    {
        m_KeyboardSO.m_onAppendString.UnityEvent.RemoveListener(AppendText);
        m_KeyboardSO.m_DoneTypingEvent.UnSubscribeListener(m_VoidEventListener);
        editing = false;
    }

    public string GetCurrentText()
    {
        return m_text;
    }

    IEnumerator EditingText()
    {
        while (editing)
        {
            m_TextComp.text = m_text;

            yield return new WaitForSeconds(0.2f);

            m_TextComp.text = m_text;

            if (m_KeyboardSO.m_insertTextIndex < m_text.Length)
                m_TextComp.text = m_TextComp.text.Insert(m_KeyboardSO.m_insertTextIndex, "|");
            else
                m_TextComp.text += "|";

            yield return new WaitForSeconds(0.2f);
        }

        m_TextComp.text = m_text;
    }


}
