using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public event OnCollisionEvent collisionCallback;
    public delegate void OnCollisionEvent();

    void Start()
    {
        collisionCallback += UIManager.instance.OnObjectSelected;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if(other.tag.ToLower() == "selectable")
        {
            collisionCallback();
        }
    }
}
