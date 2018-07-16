using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject HeldObject;

    public event OnCollisionEvent collisionCallback;
    public delegate void OnCollisionEvent(GameObject value);

    void Start()
    {
        collisionCallback += UIManager.instance.OnObjectSelected;
        if (UIManager.instance.hand == null)
            UIManager.instance.hand = this;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if(other.tag.ToLower() == "selectable")
        {
            GameObject annotatingObject = other.gameObject;
            collisionCallback(annotatingObject);
            other.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void Update()
    {
        if ( HeldObject != null)
        {
            //TODO: once key bindings are in place check to see if the release button is pressed
            //and if it is call releasedObject()
            //if(Controller.GetRelease())
            //{
                  ReleaseObject();
            //}
        }
    }

    public void GrabObject(GameObject grabbedObject)
    {
        HeldObject = grabbedObject;

        HeldObject.transform.parent = transform;
    }

    public void ReleaseObject()
    {
        HeldObject.transform.parent = null;
        Annotation annotation = HeldObject.GetComponent<Annotation>();
        annotation.linrenderer.SetPosition(0, annotation.transform.position);
        annotation.linrenderer.SetPosition(1, transform.position);
        HeldObject = null;
    }
}
