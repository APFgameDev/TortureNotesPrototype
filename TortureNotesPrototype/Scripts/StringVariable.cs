using UnityEngine;

namespace NewNet
{
    /// <summary>ScriptableObject container for a string</summary>
    [CreateAssetMenu(fileName = "StringVar", menuName = "SO Variables/String", order = 5)]
    public class StringVariable : ScriptableObject
    {
        [SerializeField]
        string m_Value;
        public string Value
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
        public string ResetValue;
        /// <summary>Invoked whenever the variable's Value is changed</summary>
        public event System.Action ValueChanged = delegate { };

        public void SetValue(string a_value)
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