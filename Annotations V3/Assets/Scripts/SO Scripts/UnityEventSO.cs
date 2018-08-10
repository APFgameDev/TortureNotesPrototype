using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Annotation.SO.UnityEvents
{
    [CreateAssetMenu(fileName = "New UnityEventVoidSO", menuName = "UnityEvent SO/Void")]
    public class UnityEventVoidSO : ScriptableObject { public readonly UnityEvent UnityEvent = new UnityEvent(); }

}