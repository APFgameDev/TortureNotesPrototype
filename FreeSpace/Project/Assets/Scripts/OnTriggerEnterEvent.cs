using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class CollisionEvent : UnityEvent<Collider>
{
}

public class OnTriggerEnterEvent : MonoBehaviour
{
    [SerializeField]
    CollisionEvent m_unityEvent;

    private void OnTriggerEnter(Collider other)
    {
        m_unityEvent.Invoke(other);
    }
}
