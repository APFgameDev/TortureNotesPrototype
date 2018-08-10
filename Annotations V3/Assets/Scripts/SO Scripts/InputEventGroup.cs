using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Annotation.SO
{
    public class InputEventGroup : ScriptableObject
    {
        public UnityEvents.UnityEventVoidSO OnPressed;
        public UnityEvents.UnityEventVoidSO OnHeld;
        public UnityEvents.UnityEventVoidSO OnReleased;
        public SO.BoolVariable inputPressed;
    }
}
