using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Annotation.NS_SO
{
    [CreateAssetMenu(fileName = "ColorSO", menuName = "Keyboard SO/Color", order = 1)]
    public class ColorSO : ScriptableObject
    {
        public Color m_Value;
    }
}