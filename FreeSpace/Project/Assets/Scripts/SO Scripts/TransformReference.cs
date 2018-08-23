using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Annotation.SO
{
    /// <summary>ScriptableObject container for a string</summary>
    [CreateAssetMenu(fileName = "TransformRefrence", menuName = "SO Variables/TransformRefrence")]
    public class TransformRefrence : ScriptableObject
    {
        [SerializeField]
        Transform m_Value;
        public Transform Value
        {
            get { return m_Value; }
            set
            {
                if (m_Value != value)
                {
                    m_Value = value;
                    ValueChanged();
                }
            }
        }
        public bool ResetOnEnable;
        public Transform ResetValue;
        /// <summary>Invoked whenever the variable's Value is changed</summary>
        public event System.Action ValueChanged = delegate { };

        public void SetValue(Transform a_value)
        {
            Value = a_value;
        }

        private void OnValidate()
        {
            ValueChanged();
        }

        private void OnEnable()
        {
            ValueChanged = delegate { };

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += ResetEventOnExitPlaymode;
#endif //UNITY_EDITOR

            if (ResetOnEnable)
            {
                Value = ResetValue;
            }
        }

#if UNITY_EDITOR
        private void ResetEventOnExitPlaymode(UnityEditor.PlayModeStateChange a_NewState)
        {
            if (a_NewState == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                ValueChanged = delegate { };
            }
        }
#endif //UNITY_EDITOR
    }
}