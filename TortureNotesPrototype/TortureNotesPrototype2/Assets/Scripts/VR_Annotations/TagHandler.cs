using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Inherite from VR Moveable
public class TagHandler : MonoBehaviour
{
    Transform target;
    [SerializeField]
    LineRenderer line;
    [SerializeField]
    Text text;

    private void Awake()
    {
    }

    public void PlaceTag(Vector3 pos,Transform aTarget)
    {
        transform.position = pos;
        target = aTarget;
    }

    private void Update()
    {
        if(target)
            line.SetPositions(new Vector3[] { transform.position, target.position });
    }

}
