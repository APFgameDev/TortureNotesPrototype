using UnityEngine;

namespace NS_Annotation.NS_SO
{
    /// <summary>ScriptableObject container for an int</summary>
    [CreateAssetMenu(fileName = "IntVar", menuName = "SO Variables/Int", order = 3)]
    public class IntVariable : ScriptableObject
    {
        [SerializeField]
        int m_Value;
        public int Value
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
        public int ResetValue;
        /// <summary>Invoked whenever the variable's Value is changed</summary>
        public event System.Action ValueChanged = delegate { };

        public void SetValue(int a_value)
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