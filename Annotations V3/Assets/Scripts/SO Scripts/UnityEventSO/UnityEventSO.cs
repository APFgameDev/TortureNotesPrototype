using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Annotation.SO.UnityEvents
{
    public class UnityEventSO<T> : ScriptableObject { public readonly UnityTemplateEvent<T> UnityEvent = new UnityTemplateEvent<T>(); }

    public class UnityTemplateEvent<T> : UnityEvent<T>
    {
        protected override MethodInfo FindMethod_Impl(string name, object targetObj)
        {
            return base.FindMethod_Impl(name, targetObj);
        }
    }
}