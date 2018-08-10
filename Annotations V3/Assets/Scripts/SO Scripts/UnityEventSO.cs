using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Annotation.SO.UnityEvents
{
    public class UnityEventSO<T> : ScriptableObject { public readonly UnityTemplateEvent<T> UnityEvent = new UnityTemplateEvent<T>(); }

    [CreateAssetMenu(fileName = "New UnityEventVoidSO", menuName = "UnityEvent SO/Void")]
    public class UnityEventVoidSO : ScriptableObject { public readonly UnityEvent UnityEvent = new UnityEvent(); }

    [CreateAssetMenu(fileName = "New UnityEventIntSO", menuName = "UnityEvent SO/Int")]
    public class UnityEventIntSO : UnityEventSO<int> {}

    public class UnityTemplateEvent<T> : UnityEvent<T>
    {
        protected override MethodInfo FindMethod_Impl(string name, object targetObj)
        {
            return base.FindMethod_Impl(name, targetObj);
        }
    }
}