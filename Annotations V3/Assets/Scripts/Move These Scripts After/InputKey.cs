using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Annotation.NS_SO;

public class InputKey : KeyboardKey
{
    [Header("Are we Caps?")]
    [SerializeField]
    private BoolVariable m_IsCaps;

    [Header("What the key is going to input")]
    [SerializeField]
    private string m_InputValue;

    protected override void Awake()
    {
        base.Awake();

        m_IsCaps.ValueChanged += OnCapsChange;
    }

    protected override void OnHit()
    {

    }

    /// <summary>
    /// Used to tell the keys when the caps has been changed
    /// </summary>
    private void OnCapsChange()
    {
        //Update visuals
    }
}
