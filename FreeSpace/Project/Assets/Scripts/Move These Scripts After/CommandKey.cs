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
        if (m_IsInKeyboardOptions.Value == true && m_OverrideKeyboardOptions == false)
        {
            return;
        }

        if (m_Command != null)
        {
            m_Command.Invoke();

            if (m_AnimateKey == true)
            {
                Vector3 newPos = new Vector3(m_KeyGeometry.transform.localPosition.x, m_KeyPressYPos, m_KeyGeometry.transform.localPosition.z);
                m_KeyGeometry.transform.localPosition = newPos;
            }
        }
        else
        {
            Debug.LogError("Command not set in key " + gameObject.name);
        }
    }
}
