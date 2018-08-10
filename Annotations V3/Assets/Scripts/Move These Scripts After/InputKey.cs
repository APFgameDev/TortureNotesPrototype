using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;
using TMPro;

public class InputKey : KeyboardKey
{
    [Header("Are we Caps?")]
    [SerializeField]
    private BoolVariable m_IsCaps;

    [Header("What the key is going to input")]
    [SerializeField]
    private string m_InputValue;

    [Header("Want the visual text to be caps when caps?")]
    [SerializeField]
    private bool m_CapsText = true;

    private TextMeshProUGUI Text;

    protected override void Awake()
    {
        base.Awake();

        Text = GetComponentInChildren<TextMeshProUGUI>();

        m_IsCaps.ValueChanged += OnCapsChange;
    }

    private void OnValidate()
    {
        TextMeshProUGUI tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = m_InputValue;
    }

    protected override void OnHit()
    {
        m_KeyboardSO.AppendString(m_InputValue);
    }

    /// <summary>
    /// Used to tell the keys when the caps has been changed
    /// </summary>
    private void OnCapsChange()
    {
        //Update visuals
        if(m_CapsText == true)
        {
            Text.text = m_IsCaps.Value ? Text.text.ToUpper() : Text.text.ToLower();
        }
    }
}
