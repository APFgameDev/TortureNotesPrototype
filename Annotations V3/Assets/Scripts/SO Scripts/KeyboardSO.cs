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

        public VoidEvent m_DoneTypingEvent;
        public VoidEvent m_TurnOffEvent;
        public VoidEvent m_TurnOnEvent;

        [SerializeField]
        private StringVariable m_KeyboardInputSO;

        /// <summary>
        /// Will add the passed in character string to the KeyboardInputSO
        /// </summary>
        /// <param name="character"></param>
        public void AppendString(string character)
        {
            m_KeyboardInputSO.Value += m_IsCaps.Value ? character.ToUpper() : character.ToLower();
        }

        public void RemoveString()
        {
            string value = m_KeyboardInputSO.Value;

            if (value != string.Empty)
            {
                value = value.Substring(0, value.Length - 1);
            }

            m_KeyboardInputSO.Value = value;
        }

        public void InvokeTurnOn()
        {
            m_KeyboardInputSO.Value = string.Empty;
            m_TurnOnEvent.Publish();
        }

        public void InvokeTurnOff()
        {
            m_TurnOffEvent.Publish();
        }
    }
}