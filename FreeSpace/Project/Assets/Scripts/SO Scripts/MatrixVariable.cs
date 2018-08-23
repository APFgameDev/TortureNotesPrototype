using UnityEngine;

namespace Annotation.SO
{
    /// <summary>ScriptableObject container for a float </summary>
    [CreateAssetMenu(fileName = "MatrixVar", menuName = "SO Variables/Matrix", order = 5)]
    public class MatrixVariable : ScriptableObject
    {
        [SerializeField]
        Matrix4x4 m_Value;

        public Matrix4x4 Value
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
        public Matrix4x4 ResetValue;
        /// <summary> Invoked whenever the variable's Value is changed </summary>
        public System.Action ValueChanged = delegate { };

        public void SetValue(Matrix4x4 a_value)
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