using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Annotation.SO
{
    [CreateAssetMenu(fileName = "ColorSO", menuName = "Keyboard SO/Color", order = 1)]
    public class ColorSO : ScriptableObject
    {
        public Color m_Value;
    }
}