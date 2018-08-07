using UnityEngine;

namespace Annotation.SO
{
    /// <summary>ScriptableObject container for a Vector2</summary>
    [CreateAssetMenu(fileName = "Vector2Var", menuName = "SO Variables/Vector2", order = 5)]
    public class Vector2Variable : ScriptableObject
    {
        [SerializeField]
        Vector2 m_Value;

        public Vector2 Value
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
        public Vector2 ResetValue;
        /// <summary>Invoked whenever the variable's Value is changed</summary>
        public System.Action ValueChanged = delegate { };

        public void SetValue(Vector2 a_value)
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