using Annotation.SO;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VRText : MonoBehaviour
{
    [SerializeField]
    private KeyboardSO m_KeyboardSO;
    [SerializeField]
    private StringVariable m_InputString;

    private TextMeshProUGUI m_TextComp;

    private void OnEnable()
    {
        if (m_KeyboardSO == null || m_InputString == null)
        {
            Debug.LogError("The keyboard so / input string variable are not set. Please set these.", this);
        }

        if (m_TextComp == null) m_TextComp = GetComponent<TextMeshProUGUI>();
    }

    public void StartListening()
    {
        m_KeyboardSO.m_OnPublish.AddListener(AppendText);
    }

    public void AppendText()
    {
        m_TextComp.text = m_InputString.Value;
        m_KeyboardSO.m_OnPublish.RemoveListener(AppendText);
    }
}
