using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventInvoker : MonoBehaviour
{
    public UnityEvent UnityEvent = new UnityEvent();

    public void Invoke()
    {
        UnityEvent.Invoke();
    }
}
