using UnityEngine;
using UnityEngine.Events;
using Annotation.SO;

namespace Annotation.SO
{
    [CreateAssetMenu(fileName = "KeyboardSO", menuName = "Keyboard SO/Keyboard", order = 0)]
    public class KeyboardSO : ScriptableObject
    {
        //public UnityEvent m_OnTurnOn;
        //public UnityEvent m_OnTurnOff;
        //public UnityEvent m_OnPublish;
        public BoolVariable m_IsCaps;
        public BoolVariable m_InOptionsMenu;

        public VoidEvent m_DoneTypingEvent;
        public VoidEvent m_TurnOffEvent;
        public VoidEvent m_TurnOnEvent;
        public UnityEvents.UnityEventStringSO m_onAppendString;

        public StringVariable m_KeyboardInputSO;
        public BoolVariable m_IsKeyboardActive;

        public Transform m_textTarget;

        public int m_insertTextIndex = 0;

        public string beforeText;

        /// <summary>
        /// Will add the passed in character string to the KeyboardInputSO
        /// </summary>
        /// <param name="character"></param>
        public void AppendString(string character)
        {
            if (m_insertTextIndex < m_KeyboardInputSO.Value.Length)
                m_KeyboardInputSO.Value = m_KeyboardInputSO.Value.Insert(m_insertTextIndex, character);
            else
                m_KeyboardInputSO.Value += m_IsCaps.Value ? character.ToUpper() : character.ToLower();

            if (m_onAppendString != null)
                m_onAppendString.Invoke(m_KeyboardInputSO.Value);

            m_insertTextIndex += character.Length;
        }

        public void SetStringValue(VRText vRText)
        {
            beforeText = m_KeyboardInputSO.Value = vRText.GetCurrentText();
            m_insertTextIndex = m_KeyboardInputSO.Value.Length;
        }

        public void SetStringValueEmpty()
        {
            beforeText = m_KeyboardInputSO.Value = string.Empty;
            m_insertTextIndex = m_KeyboardInputSO.Value.Length;
        }

        public void RemoveString()
        {
            string value = m_KeyboardInputSO.Value;

            if (value != string.Empty && m_insertTextIndex > 0)
            {
                value = value.Remove(m_insertTextIndex - 1, 1);
                m_insertTextIndex--;
            }

            m_KeyboardInputSO.Value = value;

            if (m_onAppendString != null)
                m_onAppendString.Invoke(m_KeyboardInputSO.Value);
        }

        public void ClearString()
        {
            m_KeyboardInputSO.Value = string.Empty;
            m_insertTextIndex = 0;
            if (m_onAppendString != null)
                m_onAppendString.Invoke(m_KeyboardInputSO.Value);
        }

        public void InvokeTurnOn()
        {
            m_insertTextIndex = 0;
            m_KeyboardInputSO.Value = string.Empty;
            m_TurnOnEvent.Publish();
            m_IsKeyboardActive.Value = true;
        }

        public void InvokeTurnOff()
        {
            m_TurnOffEvent.Publish();
            m_IsKeyboardActive.Value = false;
        }
    }
}