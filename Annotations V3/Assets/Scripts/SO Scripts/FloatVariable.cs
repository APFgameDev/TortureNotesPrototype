using UnityEngine;

namespace Brinx.SO
{
    /// <summary>ScriptableObject container for a float </summary>
    [CreateAssetMenu(fileName = "FloatVar", menuName = "SO Variables/Float", order = 4)]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField]
        float m_Value;

        public float Value
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

        public bool ResetOnEnabled;
        public float ResetValue;
        /// <summary> Invoked whenever the variable's Value is changed </summary>
        public System.Action ValueChanged = delegate { };

        public void SetValue(float a_value)
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
#endif

            if (ResetOnEnabled)
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

#endif
    }
}
