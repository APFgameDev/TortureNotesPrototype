using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;
using TMPro;

public class TextInjector : MonoBehaviour
{
    public StringVariable m_KeyboardInputSO;
    public TextMeshProUGUI m_TextMeshObj;




    private void Update()
    {
        m_TextMeshObj.text = m_KeyboardInputSO.Value;
    }
}
