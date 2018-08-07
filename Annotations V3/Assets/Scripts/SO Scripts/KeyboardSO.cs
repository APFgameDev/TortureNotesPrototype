using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NS_Annotation.NS_SO;
using Annotation.SO;

namespace NS_Annotation.NS_SO
{
    [CreateAssetMenu(fileName = "KeyboardSO", menuName = "Keyboard SO/Keyboard", order = 0)]
    public class KeyboardSO : ScriptableObject
    {
        public UnityEvent m_OnTurnOn;
        public UnityEvent m_OnTurnOff;
        public UnityEvent m_OnPublish;

        [SerializeField]
        private StringVariable m_KeyboardInputSO;

        [SerializeField]
        private BoolVariable m_IsCaps;

        private void OnEnable()
        {
            Reset();
        }

        /// <summary>
        /// Clears all listeners from the events
        /// </summary>
        public void Reset()
        {
            m_OnTurnOn.RemoveAllListeners();
            m_OnTurnOff.RemoveAllListeners();
            m_OnPublish.RemoveAllListeners();
        }

        /// <summary>
        /// Will add the passed in character string to the KeyboardInputSO
        /// </summary>
        /// <param name="character"></param>
        public void AppendString(string character)
        {
            m_KeyboardInputSO.Value += m_IsCaps.Value ? character.ToUpper() : character.ToLower();
        }
    }
}