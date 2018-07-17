using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject HeldObject;

    public event OnCollisionEvent collisionCallback;
    public delegate void OnCollisionEvent(GameObject value);

    [SerializeField]
    private GameObject imdepressed;
    private bool selected;

    //Vector3 edgePos;

    void Start()
    {
        collisionCallback += UIManager.instance.OnObjectSelected;
        if (UIManager.instance.hand == null)
            UIManager.instance.hand = this;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if(other.tag.ToLower() == "selectable" && !selected)
        {
            selected = true;
            imdepressed = other.gameObject;            
            collisionCallback(imdepressed);
            other.GetComponent<Renderer>().material.color = Color.red;

            // Ray Cast To Find edge 
           // RaycastHit raycastHit;
           // imdepressed.GetComponent<Collider>().Raycast(new Ray(transform.position, (imdepressed.transform.position - transform.position).normalized), out raycastHit, 100);
           // edgePos = raycastHit.point;

        }
    }

    private void Update()
    {
        if ( HeldObject != null)
        {
            if(Input.GetAxis(InputAxis.RightGripTrigger) > 0.5f)
            {
                  ReleaseObject();
            }
        }
    }

    public void GrabObject(GameObject grabbedObject)
    {
        HeldObject = grabbedObject;

        HeldObject.transform.parent = transform;
    }

    public void ReleaseObject()
    {
        Annotation annotation = HeldObject.GetComponent<Annotation>();
        annotation.linrenderer.SetPosition(0, annotation.transform.position);

         //annotation.linrenderer.SetPosition(1,edgePos);
        annotation.linrenderer.SetPosition(1, imdepressed.transform.position);

        selected = false;
        HeldObject.transform.parent = null;
        HeldObject = null;
        imdepressed = null;
    }
}
