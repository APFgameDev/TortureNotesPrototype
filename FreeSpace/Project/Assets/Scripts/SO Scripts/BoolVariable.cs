using UnityEngine;

namespace Annotation.SO
{
    /// <summary>ScriptableObject container for a bool</summary>
    [CreateAssetMenu(fileName = "BoolVar", menuName = "SO Variables/Bool", order = 0)]
    public class BoolVariable : ScriptableObject
    {
        [SerializeField]
        bool m_Value;
        public bool Value
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
        public bool ResetValue;
        /// <summary>Invoked whenever the variable's Value is changed</summary>
        public event System.Action ValueChanged = delegate { };

        public void SetValue(bool a_value)
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
};