using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;
using TMPro;

public class InputKey : KeyboardKey
{
    [Header("Are we Caps?")]
    [SerializeField] private BoolVariable m_IsCaps;

    [Header("What the key is going to input")]
    [SerializeField] private string m_InputValue;

    [Header("Want the visual text to be caps when caps?")]
    [SerializeField] private bool m_CapsText = true;

    [Header("Want the key text to change to its input?")]
    [SerializeField] private bool m_KeyChangeToInput = true;

    [Header("Text object on canvas on key")]
    [SerializeField] private TextMeshProUGUI Text;


    protected override void Awake()
    {
        base.Awake();

        Text = transform.parent.GetComponentInChildren<TextMeshProUGUI>();

        m_IsCaps.ValueChanged += OnCapsChange;
    }

    private void OnEnable()
    {
        if (Text == null)
        {
            Text = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void OnValidate()
    {
        if (m_KeyChangeToInput == true)
        {
            if (m_TextObject != null)
            {
                m_TextObject.text = m_InputValue;
            }
        }
    }

    protected override void OnHit()
    {
        m_KeyboardSO.AppendString(m_InputValue);

        if (m_AnimateKey == true)
        {
            Vector3 newPos = new Vector3(m_KeyGeometry.transform.localPosition.x, 1.5f, m_KeyGeometry.transform.localPosition.z);
            m_KeyGeometry.transform.localPosition = newPos;
        }
    }

    /// <summary>
    /// Used to tell the keys when the caps has been changed
    /// </summary>
    private void OnCapsChange()
    {
        //Update visuals
        if (m_CapsText == true)
        {
            Text.text = m_IsCaps.Value ? Text.text.ToUpper() : Text.text.ToLower();
        }
    }
}
