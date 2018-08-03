using UnityEngine;

namespace NS_Annotation.NS_SO
{
    /// <summary>ScriptableObject container for an int</summary>
    [CreateAssetMenu(fileName = "FloatRange", menuName = "SO Variables/FloatRange", order = 4)]
    public class FloatRangeSO : ScriptableObject
    {
        [Range(0.0f, 1.0f)]
        public float m_Value;
    }
}