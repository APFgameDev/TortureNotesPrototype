using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandKey : KeyboardKey
{
    [SerializeField]
    private UnityEvent m_Command;

    protected override void OnHit()
    {
        if(m_Command != null)
        {
            m_Command.Invoke();
        }
        else
        {
            Debug.LogError("Command not set in key " + gameObject.name);
        }
    }
}
