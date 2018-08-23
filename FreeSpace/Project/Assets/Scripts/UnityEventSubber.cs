using Annotation.SO.UnityEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventSubber : MonoBehaviour
{
    [SerializeField]
    UnityEventVoidSO m_event;
    [SerializeField]
    UnityEvent m_callbacks;

    public void SubscribeToUnityEvent()
    {
        m_event.UnityEvent.AddListener(m_callbacks.Invoke);
    }

    public void UnSubscribeToUnityEvent()
    {
        m_event.UnityEvent.RemoveListener(m_callbacks.Invoke);
    }
}
